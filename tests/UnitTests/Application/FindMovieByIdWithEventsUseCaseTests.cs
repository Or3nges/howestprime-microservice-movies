using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies.FindMovieByIdWithEvents;
using EntitiesMovie = Howestprime.Movies.Domain.Entities.Movie;
using EntitiesMovieEvent = Howestprime.Movies.Domain.Entities.MovieEvent;
using EntitiesRoom = Howestprime.Movies.Domain.Entities.Room;
using Xunit;

namespace UnitTests.Application
{
    public class FindMovieByIdWithEventsUseCaseTests
    {
        private class FakeMovieRepository : IMovieRepository
        {
            public EntitiesMovie? Movie;
            public Task<EntitiesMovie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult(Movie);
            public Task<EntitiesMovie> AddAsync(EntitiesMovie movie, CancellationToken cancellationToken = default) => throw new NotImplementedException();
            public Task<IEnumerable<EntitiesMovie>> FindByFiltersAsync(string? title, string? genre, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        }
        private class FakeMovieEventRepository : IMovieEventRepository
        {
            public IEnumerable<EntitiesMovieEvent> Events = new List<EntitiesMovieEvent>();
            public Task<EntitiesMovieEvent?> GetByRoomDateTimeAsync(Guid roomId, DateTime date, TimeSpan time) => throw new NotImplementedException();
            public Task AddAsync(EntitiesMovieEvent movieEvent) => throw new NotImplementedException();
            public Task DeleteAsync(Guid id) => throw new NotImplementedException();
            public Task<IEnumerable<EntitiesMovieEvent>> GetEventsForMovieInRangeAsync(Guid movieId, DateTime start, DateTime end) => Task.FromResult(Events);
            public Task<IEnumerable<EntitiesMovieEvent>> GetEventsInRangeAsync(DateTime start, DateTime end) => throw new NotImplementedException();
            public Task<EntitiesMovieEvent> GetByIdWithBookingsAsync(Guid movieEventId) => throw new NotImplementedException();
            public Task UpdateAsync(EntitiesMovieEvent movieEvent) => throw new NotImplementedException();
        }
        private class FakeRoomRepository : IRoomRepository
        {
            public EntitiesRoom Room = new EntitiesRoom { Id = Guid.NewGuid(), Name = "Room", Capacity = 10 };
            public Task<EntitiesRoom?> GetByIdAsync(Guid id) => Task.FromResult(Room as EntitiesRoom);
            public Task<EntitiesRoom> AddAsync(EntitiesRoom room) => throw new NotImplementedException();
            public Task<IEnumerable<EntitiesRoom>> GetAllAsync() => throw new NotImplementedException();
            public Task SeedRoomsAsync() => Task.CompletedTask;
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsMovieData_WhenMovieAndEventsExist()
        {
            var movieId = Guid.NewGuid();
            var movie = new EntitiesMovie(movieId, "Title", "Desc", "Genre", "Actors", "PG", 120, "url");
            var events = new List<EntitiesMovieEvent> { new EntitiesMovieEvent { Id = Guid.NewGuid(), MovieId = movieId, RoomId = Guid.NewGuid(), Date = DateTime.UtcNow, Time = TimeSpan.FromHours(15), Capacity = 10 } };
            var useCase = new FindMovieByIdWithEventsUseCase(
                new FakeMovieRepository { Movie = movie },
                new FakeMovieEventRepository { Events = events },
                new FakeRoomRepository()
            );
            var query = new FindMovieByIdWithEventsQuery { MovieId = movieId };
            var result = await useCase.ExecuteAsync(query);
            Assert.NotNull(result);
            Assert.Equal(movieId, result.Id);
            Assert.NotEmpty(result.Events);
        }

        [Fact]
        public async Task ExecuteAsync_Throws_WhenMovieNotFound()
        {
            var useCase = new FindMovieByIdWithEventsUseCase(
                new FakeMovieRepository { Movie = null },
                new FakeMovieEventRepository { Events = new List<EntitiesMovieEvent>() },
                new FakeRoomRepository()
            );
            var query = new FindMovieByIdWithEventsQuery { MovieId = Guid.NewGuid() };
            await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(query));
        }

        [Fact]
        public async Task ExecuteAsync_Throws_WhenNoEventsFound()
        {
            var movieId = Guid.NewGuid();
            var movie = new EntitiesMovie(movieId, "Title", "Desc", "Genre", "Actors", "PG", 120, "url");
            var useCase = new FindMovieByIdWithEventsUseCase(
                new FakeMovieRepository { Movie = movie },
                new FakeMovieEventRepository { Events = new List<EntitiesMovieEvent>() },
                new FakeRoomRepository()
            );
            var query = new FindMovieByIdWithEventsQuery { MovieId = movieId };
            await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(query));
        }

        [Fact]
        public async Task ExecuteAsync_MovieWithNoEvents_ThrowsException()
        {
            var movieId = Guid.NewGuid();
            var movie = new EntitiesMovie(movieId, "Title", "Desc", "Genre", "Actors", "PG", 120, "url");
            var useCase = new FindMovieByIdWithEventsUseCase(
                new FakeMovieRepository { Movie = movie },
                new FakeMovieEventRepository { Events = new List<EntitiesMovieEvent>() },
                new FakeRoomRepository()
            );
            var query = new FindMovieByIdWithEventsQuery { MovieId = movieId };
            await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(query));
        }
    }
}
