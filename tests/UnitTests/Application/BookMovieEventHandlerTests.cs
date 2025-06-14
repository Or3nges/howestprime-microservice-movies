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
using Howestprime.Movies.Domain.Events;
using Xunit;

namespace UnitTests.Application
{
    public class FakeMovieEventRepository : IMovieEventRepository
    {
        public Dictionary<MovieEventId, MovieEvent> Events { get; } = new();

        public Task<MovieEvent?> GetByIdAsync(MovieEventId id)
        {
            return Task.FromResult(Events.GetValueOrDefault(id));
        }

        public Task<MovieEvent> GetByIdWithBookingsAsync(MovieEventId id)
        {
            var movieEvent = Events.GetValueOrDefault(id);
            if (movieEvent == null)
                throw new NotFoundException($"MovieEvent with id {id} not found");
            return Task.FromResult(movieEvent);
        }

        public Task<MovieEvent?> GetByRoomDateTimeAsync(RoomId roomId, DateTime date, TimeSpan time)
        {
            return Task.FromResult(Events.Values.FirstOrDefault(e => 
                e.RoomId == roomId && 
                e.Time <= date && 
                e.Time.Add(time) >= date));
        }

        public Task<IEnumerable<MovieEvent>> GetEventsForMovieInRangeAsync(MovieId movieId, DateTime start, DateTime end)
        {
            var events = Events.Values.Where(e => 
                e.MovieId == movieId && 
                e.Time >= start && 
                e.Time <= end);
            return Task.FromResult(events);
        }

        public Task<IEnumerable<MovieEvent>> GetEventsInRangeAsync(DateTime start, DateTime end)
        {
            var events = Events.Values.Where(e => 
                e.Time >= start && 
                e.Time <= end);
            return Task.FromResult(events);
        }

        public Task SaveAsync(MovieEvent movieEvent)
        {
            Events[movieEvent.Id] = movieEvent;
            return Task.CompletedTask;
        }

        public Task AddAsync(MovieEvent movieEvent)
        {
            Events[movieEvent.Id] = movieEvent;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(MovieEvent movieEvent)
        {
            Events[movieEvent.Id] = movieEvent;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(MovieEventId id)
        {
            Events.Remove(id);
            return Task.CompletedTask;
        }

        public void Add(MovieEvent movieEvent)
        {
            Events[movieEvent.Id] = movieEvent;
        }
    }

    public class FakeUnitOfWork : IUnitOfWork
    {
        public Task CommitAsync()
        {
            return Task.CompletedTask;
        }
    }

    public class FakeEventPublisher : IEventPublisher
    {
        public List<BookingOpened> PublishedEvents { get; } = new();

        public Task PublishAsync(BookingOpened bookingOpened)
        {
            PublishedEvents.Add(bookingOpened);
            return Task.CompletedTask;
        }
    }

    public class FakeRoomRepository : IRoomRepository
    {
        public Dictionary<RoomId, Room> Rooms { get; } = new();

        public Task<Room?> GetByIdAsync(RoomId id)
        {
            return Task.FromResult(Rooms.GetValueOrDefault(id));
        }

        public Task<Room?> ById(RoomId id)
        {
            return Task.FromResult(Rooms.GetValueOrDefault(id));
        }

        public Task<IEnumerable<Room>> GetAllAsync()
        {
            return Task.FromResult(Rooms.Values.AsEnumerable());
        }

        public Task<Room> AddAsync(Room room)
        {
            Rooms[room.Id] = room;
            return Task.FromResult(room);
        }

        public Task SeedRoomsAsync()
        {
            return Task.CompletedTask;
        }

        public void Add(Room room)
        {
            Rooms[room.Id] = room;
        }
    }

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
        }

        [Fact]
        public async Task HandleAsync_EventNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var movieEventId = new MovieEventId();
            var command = new BookMovieEventCommand
            {
                MovieEventId = movieEventId,
                StandardVisitors = 3,
                DiscountVisitors = 2
            };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.HandleAsync(command));
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
        }

        [Fact]
        public async Task HandleAsync_InsufficientSeats_ThrowsInvalidOperationException()
        {
            // Arrange
            var movieEventId = new MovieEventId();
            var roomId = new RoomId();
            var room = new Room(roomId, "Test Room", 10);
            var movieEvent = new MovieEvent(movieEventId, new MovieId(), roomId, DateTime.UtcNow.AddDays(1), 10);
            
            await _roomRepository.AddAsync(room);
            await _movieEventRepository.AddAsync(movieEvent);
            
            var command = new BookMovieEventCommand
            {
                MovieEventId = movieEventId,
                StandardVisitors = 15,
                DiscountVisitors = 5
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.HandleAsync(command));
        }
    }
}
