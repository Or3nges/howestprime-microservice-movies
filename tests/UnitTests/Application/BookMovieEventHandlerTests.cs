using System;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Movies.BookMovieEvent;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Entities;
using Howestprime.Movies.Domain.Events;
using Xunit;
using System.Collections.Generic;

namespace UnitTests.Application
{
    public class BookMovieEventHandlerTests
    {
        private class FakeMovieEventRepository : IMovieEventRepository
        {
            public MovieEvent? MovieEvent;
            public bool Updated;
            public Task<MovieEvent?> GetByRoomDateTimeAsync(Guid roomId, DateTime date, TimeSpan time) => throw new NotImplementedException();
            public Task AddAsync(MovieEvent movieEvent) => throw new NotImplementedException();
            public Task DeleteAsync(Guid id) => throw new NotImplementedException();
            public Task<IEnumerable<MovieEvent>> GetEventsForMovieInRangeAsync(Guid movieId, DateTime start, DateTime end) => throw new NotImplementedException();
            public Task<IEnumerable<MovieEvent>> GetEventsInRangeAsync(DateTime start, DateTime end) => throw new NotImplementedException();
            public Task<MovieEvent> GetByIdWithBookingsAsync(Guid movieEventId) => Task.FromResult(MovieEvent!);
            public Task UpdateAsync(MovieEvent movieEvent) { Updated = true; return Task.CompletedTask; }
        }
        
        private class FakeUnitOfWork : IUnitOfWork { public Task CommitAsync() => Task.CompletedTask; }
        
        private class FakeEventPublisher : IEventPublisher { public BookingOpened? Published; public Task PublishAsync(BookingOpened e) { Published = e; return Task.CompletedTask; } }
        
        private class FakeRoomRepository : IRoomRepository 
        {
            public Room? Room { get; set; } = new Room { Id = Guid.NewGuid(), Name = "Test Room", Capacity = 100 };
            
            public Task<Room?> GetByIdAsync(Guid id) => Task.FromResult(Room);
            public Task SeedRoomsAsync() => Task.CompletedTask;
        }

        [Fact]
        public async Task HandleAsync_ValidCommand_BooksEventAndPublishes()
        {
            var movieEvent = new MovieEvent { Id = Guid.NewGuid(), Bookings = new List<Booking>(), Visitors = 0, Capacity = 10 };
            var repo = new FakeMovieEventRepository { MovieEvent = movieEvent };
            var uow = new FakeUnitOfWork();
            var publisher = new FakeEventPublisher();
            var roomRepo = new FakeRoomRepository();
            
            var handler = new BookMovieEventHandler(repo, uow, publisher, roomRepo);
            
            var command = new BookMovieEventCommand { MovieEventId = movieEvent.Id, StandardVisitors = 1, DiscountVisitors = 1, RoomName = "Room 1" };
            var result = await handler.HandleAsync(command);
            Assert.NotNull(result);
            Assert.Single(movieEvent.Bookings);
            Assert.Equal(2, movieEvent.Visitors);
            Assert.True(repo.Updated);
            Assert.NotNull(publisher.Published);
        }

        [Fact]
        public async Task HandleAsync_InvalidVisitors_Throws()
        {
            var movieEvent = new MovieEvent { Id = Guid.NewGuid(), Bookings = new List<Booking>(), Visitors = 0, Capacity = 10 };
            var repo = new FakeMovieEventRepository { MovieEvent = movieEvent };
            var uow = new FakeUnitOfWork();
            var publisher = new FakeEventPublisher();
            var roomRepo = new FakeRoomRepository();
            
            var handler = new BookMovieEventHandler(repo, uow, publisher, roomRepo);
            
            var command = new BookMovieEventCommand { MovieEventId = movieEvent.Id, StandardVisitors = -1, DiscountVisitors = 0, RoomName = "Room 1" };
            await Assert.ThrowsAsync<ArgumentException>(() => handler.HandleAsync(command));
        }

        [Fact]
        public async Task HandleAsync_OverCapacity_Throws()
        {
            var movieEvent = new MovieEvent { Id = Guid.NewGuid(), Bookings = new List<Booking>(), Visitors = 9, Capacity = 10 };
            var repo = new FakeMovieEventRepository { MovieEvent = movieEvent };
            var uow = new FakeUnitOfWork();
            var publisher = new FakeEventPublisher();
            var roomRepo = new FakeRoomRepository();
            
            var handler = new BookMovieEventHandler(repo, uow, publisher, roomRepo);
            
            var command = new BookMovieEventCommand { MovieEventId = movieEvent.Id, StandardVisitors = 2, DiscountVisitors = 0, RoomName = "Room 1" };
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
        }

        [Fact]
        public async Task HandleAsync_BookingTooFarInAdvance_Throws()
        {
            var movieEvent = new MovieEvent { Id = Guid.NewGuid(), Bookings = new List<Booking>(), Visitors = 0, Capacity = 10, Date = DateTime.UtcNow.AddDays(15) };
            var repo = new FakeMovieEventRepository { MovieEvent = movieEvent };
            var uow = new FakeUnitOfWork();
            var publisher = new FakeEventPublisher();
            var roomRepo = new FakeRoomRepository();
            
            var handler = new BookMovieEventHandler(repo, uow, publisher, roomRepo);
            
            var command = new BookMovieEventCommand { MovieEventId = movieEvent.Id, StandardVisitors = 1, DiscountVisitors = 0, RoomName = "Room 1" };
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
        }

        [Fact]
        public async Task HandleAsync_ZeroVisitors_Throws()
        {
            var movieEvent = new MovieEvent { Id = Guid.NewGuid(), Bookings = new List<Booking>(), Visitors = 0, Capacity = 10 };
            var repo = new FakeMovieEventRepository { MovieEvent = movieEvent };
            var uow = new FakeUnitOfWork();
            var publisher = new FakeEventPublisher();
            var roomRepo = new FakeRoomRepository();
            
            var handler = new BookMovieEventHandler(repo, uow, publisher, roomRepo);
            
            var command = new BookMovieEventCommand { MovieEventId = movieEvent.Id, StandardVisitors = 0, DiscountVisitors = 0, RoomName = "Room 1" };
            await Assert.ThrowsAsync<ArgumentException>(() => handler.HandleAsync(command));
        }

        [Fact]
        public async Task HandleAsync_MovieEventNotFound_Throws()
        {
            var repo = new FakeMovieEventRepository { MovieEvent = null };
            var uow = new FakeUnitOfWork();
            var publisher = new FakeEventPublisher();
            var roomRepo = new FakeRoomRepository();
            
            var handler = new BookMovieEventHandler(repo, uow, publisher, roomRepo);
            
            var command = new BookMovieEventCommand
            {
                MovieEventId = Guid.NewGuid(),
                StandardVisitors = 1,
                DiscountVisitors = 1,
                RoomName = "Blue Room"
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
        }
    }
}
