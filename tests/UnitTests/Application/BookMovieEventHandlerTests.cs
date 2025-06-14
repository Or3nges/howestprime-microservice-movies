using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Movies.BookMovieEvent;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.MovieEvent;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Domain.Room;
using Howestprime.Movies.Domain.Shared.Exceptions;
using UnitTests.Shared;
using Xunit;

namespace UnitTests.Application
{
    public class BookMovieEventHandlerTests
    {
        private readonly FakeMovieEventRepository _movieEventRepository;
        private readonly FakeUnitOfWork _unitOfWork;
        private readonly FakeEventPublisher _eventPublisher;
        private readonly FakeRoomRepository _roomRepository;
        private readonly BookMovieEventHandler _handler;

        public BookMovieEventHandlerTests()
        {
            _movieEventRepository = new FakeMovieEventRepository();
            _unitOfWork = new FakeUnitOfWork();
            _eventPublisher = new FakeEventPublisher();
            _roomRepository = new FakeRoomRepository();
            _handler = new BookMovieEventHandler(_movieEventRepository, _unitOfWork, _eventPublisher, _roomRepository);
        }

        [Fact]
        public async Task HandleAsync_ValidBooking_Succeeds()
        {
            // Arrange
            var movieEventId = new MovieEventId();
            var roomId = new RoomId();
            var room = new Room(roomId, "Test Room", 100);
            var movieEvent = new MovieEvent(movieEventId, new MovieId(), roomId, DateTime.UtcNow.AddDays(1), 100);
            
            await _roomRepository.AddAsync(room);
            await _movieEventRepository.AddAsync(movieEvent);
            
            var command = new BookMovieEventCommand
            {
                MovieEventId = movieEventId,
                StandardVisitors = 3,
                DiscountVisitors = 2
            };

            // Act
            await _handler.HandleAsync(command);

            // Assert
            var updatedEvent = await _movieEventRepository.GetByIdAsync(movieEventId);
            Assert.NotNull(updatedEvent);
            Assert.Single(updatedEvent.Bookings);
            var booking = updatedEvent.Bookings.First();
            Assert.Equal(3, booking.StandardVisitors);
            Assert.Equal(2, booking.DiscountVisitors);
            Assert.Equal(5, booking.SeatNumbers.Count);
            Assert.Single(_eventPublisher.PublishedEvents);
            Assert.True(_unitOfWork.Committed);
        }

        [Fact]
        public async Task HandleAsync_EventNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var movieEventId = new MovieEventId();
            var command = new BookMovieEventCommand
            {
                MovieEventId = movieEventId,
                StandardVisitors = 1,
                DiscountVisitors = 0
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.HandleAsync(command));
            Assert.False(_unitOfWork.Committed);
        }

        [Fact]
        public async Task HandleAsync_InvalidVisitorCount_ThrowsArgumentException()
        {
            // Arrange
            var movieEventId = new MovieEventId();
            var command = new BookMovieEventCommand
            {
                MovieEventId = movieEventId,
                StandardVisitors = -1,
                DiscountVisitors = 0
            };
            
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.HandleAsync(command));
            Assert.False(_unitOfWork.Committed);
        }

        [Fact]
        public async Task HandleAsync_InsufficientSeats_ThrowsInvalidOperationException()
        {
            // Arrange
            var movieEventId = new MovieEventId();
            var movieEvent = new MovieEvent(movieEventId, new MovieId(), new RoomId(), DateTime.UtcNow.AddDays(1), 5);
            await _movieEventRepository.AddAsync(movieEvent);
            var command = new BookMovieEventCommand
            {
                MovieEventId = movieEventId,
                StandardVisitors = 4,
                DiscountVisitors = 2
            };
            
            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.HandleAsync(command));
            Assert.False(_unitOfWork.Committed);
        }
    }
}
