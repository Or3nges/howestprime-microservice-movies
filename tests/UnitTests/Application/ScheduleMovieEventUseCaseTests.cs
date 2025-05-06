using System;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Movies.ScheduleMovieEvent;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Entities;
using Xunit;
using System.Collections.Generic;
using System.Threading;

namespace UnitTests.Application
{
    public class ScheduleMovieEventUseCaseTests
    {
        private class FakeMovieRepository : IMovieRepository
        {
            public Movie? Movie;
            public Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult(Movie);
            public Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken = default) => throw new NotImplementedException();
            public Task<IEnumerable<Movie>> FindByFiltersAsync(string? title, string? genre, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        }
        private class FakeRoomRepository : IRoomRepository
        {
            public Room? Room;
            public Task<Room?> GetByIdAsync(Guid id) => Task.FromResult(Room);
            public Task<Room> AddAsync(Room room) => throw new NotImplementedException();
            public Task<IEnumerable<Room>> GetAllAsync() => throw new NotImplementedException();
            public Task SeedRoomsAsync() => Task.CompletedTask;
        }
        private class FakeMovieEventRepository : IMovieEventRepository
        {
            public MovieEvent? Existing;
            public MovieEvent? Added;
            public Guid? DeletedId;
            public Task<MovieEvent?> GetByRoomDateTimeAsync(Guid roomId, DateTime date, TimeSpan time) => Task.FromResult(Existing);
            public Task AddAsync(MovieEvent movieEvent) { Added = movieEvent; return Task.CompletedTask; }
            public Task DeleteAsync(Guid id) { DeletedId = id; return Task.CompletedTask; }
            public Task<IEnumerable<MovieEvent>> GetEventsForMovieInRangeAsync(Guid movieId, DateTime start, DateTime end) => throw new NotImplementedException();
            public Task<IEnumerable<MovieEvent>> GetEventsInRangeAsync(DateTime start, DateTime end) => throw new NotImplementedException();
            public Task<MovieEvent> GetByIdWithBookingsAsync(Guid movieEventId) => throw new NotImplementedException();
            public Task UpdateAsync(MovieEvent movieEvent) => throw new NotImplementedException();
        }
        [Fact]
        public async Task ExecuteAsync_InvalidTime_ThrowsException()
        {
            var useCase = new ScheduleMovieEventUseCase(
                new FakeMovieRepository { Movie = new Movie(Guid.NewGuid(), "T", "", "", "", "PG", 120, "") },
                new FakeMovieEventRepository(),
                new FakeRoomRepository { Room = new Room { Id = Guid.NewGuid(), Name = "Room", Capacity = 10 } }
            );
            var command = new ScheduleMovieEventCommand
            {
                MovieId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Date = DateTime.UtcNow.AddDays(1),
                Time = TimeSpan.FromHours(10), // Invalid
                Capacity = 10
            };
            await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(command));
        }
        [Fact]
        public async Task ExecuteAsync_PastDate_ThrowsException()
        {
            var useCase = new ScheduleMovieEventUseCase(
                new FakeMovieRepository { Movie = new Movie(Guid.NewGuid(), "T", "", "", "", "PG", 120, "") },
                new FakeMovieEventRepository(),
                new FakeRoomRepository { Room = new Room { Id = Guid.NewGuid(), Name = "Room", Capacity = 10 } }
            );
            var command = new ScheduleMovieEventCommand
            {
                MovieId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Date = DateTime.UtcNow.AddDays(-1), // Past
                Time = TimeSpan.FromHours(15),
                Capacity = 10
            };
            await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(command));
        }
        [Fact]
        public async Task ExecuteAsync_InvalidCapacity_ThrowsException()
        {
            var useCase = new ScheduleMovieEventUseCase(
                new FakeMovieRepository { Movie = new Movie(Guid.NewGuid(), "T", "", "", "", "PG", 120, "") },
                new FakeMovieEventRepository(),
                new FakeRoomRepository { Room = new Room { Id = Guid.NewGuid(), Name = "Room", Capacity = 10 } }
            );
            var command = new ScheduleMovieEventCommand
            {
                MovieId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Date = DateTime.UtcNow.AddDays(1),
                Time = TimeSpan.FromHours(15),
                Capacity = 0 // Invalid
            };
            await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(command));
        }

        [Fact]
        public async Task ExecuteAsync_MovieNotFound_ThrowsException()
        {
            var useCase = new ScheduleMovieEventUseCase(
                new FakeMovieRepository { Movie = null },
                new FakeMovieEventRepository(),
                new FakeRoomRepository { Room = new Room { Id = Guid.NewGuid(), Name = "Room", Capacity = 10 } }
            );
            var command = new ScheduleMovieEventCommand
            {
                MovieId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Date = DateTime.UtcNow.AddDays(1),
                Time = TimeSpan.FromHours(15),
                Capacity = 10
            };
            await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(command));
        }

        [Fact]
        public async Task ExecuteAsync_RoomNotFound_ThrowsException()
        {
            var useCase = new ScheduleMovieEventUseCase(
                new FakeMovieRepository { Movie = new Movie(Guid.NewGuid(), "T", "", "", "", "PG", 120, "") },
                new FakeMovieEventRepository(),
                new FakeRoomRepository { Room = null }
            );
            var command = new ScheduleMovieEventCommand
            {
                MovieId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Date = DateTime.UtcNow.AddDays(1),
                Time = TimeSpan.FromHours(15),
                Capacity = 10
            };
            await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(command));
        }

        [Fact]
        public async Task ExecuteAsync_ExistingEvent_IsDeletedAndReplaced()
        {
            var existingEvent = new MovieEvent { Id = Guid.NewGuid(), RoomId = Guid.NewGuid(), Date = DateTime.UtcNow.AddDays(1), Time = TimeSpan.FromHours(15) };
            var repo = new FakeMovieEventRepository { Existing = existingEvent };
            var useCase = new ScheduleMovieEventUseCase(
                new FakeMovieRepository { Movie = new Movie(Guid.NewGuid(), "T", "", "", "", "PG", 120, "") },
                repo,
                new FakeRoomRepository { Room = new Room { Id = existingEvent.RoomId, Name = "Room", Capacity = 10 } }
            );
            var command = new ScheduleMovieEventCommand
            {
                MovieId = Guid.NewGuid(),
                RoomId = existingEvent.RoomId,
                Date = existingEvent.Date,
                Time = existingEvent.Time,
                Capacity = 10
            };
            var result = await useCase.ExecuteAsync(command);
            Assert.Equal(repo.Added.Id, result.EventId);
            Assert.Equal(existingEvent.Id, repo.DeletedId);
        }

        [Fact]
        public async Task ExecuteAsync_SuccessfulEventScheduling_ReturnsEventId()
        {
            var repo = new FakeMovieEventRepository();
            var useCase = new ScheduleMovieEventUseCase(
                new FakeMovieRepository { Movie = new Movie(Guid.NewGuid(), "T", "", "", "", "PG", 120, "") },
                repo,
                new FakeRoomRepository { Room = new Room { Id = Guid.NewGuid(), Name = "Room", Capacity = 10 } }
            );
            var command = new ScheduleMovieEventCommand
            {
                MovieId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Date = DateTime.UtcNow.AddDays(1),
                Time = TimeSpan.FromHours(15),
                Capacity = 10
            };
            var result = await useCase.ExecuteAsync(command);
            Assert.NotEqual(Guid.Empty, result.EventId);
            Assert.NotNull(repo.Added);
        }

        [Fact]
        public async Task ExecuteAsync_NullCommand_ThrowsException()
        {
            var useCase = new ScheduleMovieEventUseCase(
                new FakeMovieRepository { Movie = new Movie(Guid.NewGuid(), "T", "", "", "", "PG", 120, "") },
                new FakeMovieEventRepository(),
                new FakeRoomRepository { Room = new Room { Id = Guid.NewGuid(), Name = "Room", Capacity = 10 } }
            );
            await Assert.ThrowsAsync<ArgumentNullException>(() => useCase.ExecuteAsync(null));
        }
    }
}
