using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies.FindMoviesWithEventsInTimeframe;
using EntitiesMovie = Howestprime.Movies.Domain.Entities.Movie;
using EntitiesMovieEvent = Howestprime.Movies.Domain.Entities.MovieEvent;
using Xunit;

namespace UnitTests.Application
{
    public class FindMoviesWithEventsInTimeframeUseCaseTests
    {
        private class FakeMovieRepository : IMovieRepository
        {
            public IEnumerable<EntitiesMovie> Movies = new List<EntitiesMovie>();
            public Task<EntitiesMovie?> GetByIdAsync(MovieId id, CancellationToken cancellationToken = default) => throw new NotImplementedException();
            public Task<EntitiesMovie> AddAsync(EntitiesMovie movie, CancellationToken cancellationToken = default) => throw new NotImplementedException();
            public Task<IEnumerable<EntitiesMovie>> FindByFiltersAsync(string? title, string? genre, CancellationToken cancellationToken = default) => Task.FromResult(Movies);
        }
        private class FakeMovieEventRepository : IMovieEventRepository
        {
            public IEnumerable<EntitiesMovieEvent> Events = new List<EntitiesMovieEvent>();
            public Task<EntitiesMovieEvent?> GetByRoomDateTimeAsync(RoomId roomId, DateTime date, TimeSpan time) => throw new NotImplementedException();
            public Task AddAsync(EntitiesMovieEvent movieEvent) => throw new NotImplementedException();
            public Task DeleteAsync(Guid id) => throw new NotImplementedException();
            public Task<IEnumerable<EntitiesMovieEvent>> GetEventsForMovieInRangeAsync(MovieId movieId, DateTime start, DateTime end) => Task.FromResult(Events);
            public Task<IEnumerable<EntitiesMovieEvent>> GetEventsInRangeAsync(DateTime start, DateTime end) => throw new NotImplementedException();
            public Task<EntitiesMovieEvent> GetByIdWithBookingsAsync(Guid movieEventId) => throw new NotImplementedException();
            public Task UpdateAsync(EntitiesMovieEvent movieEvent) => throw new NotImplementedException();
        }
        private class FakeRoomRepository : IRoomRepository
        {
            public Room Room = new Room { Id = Guid.NewGuid(), Name = "Room", Capacity = 10 };
            public Task<Room?> GetByIdAsync(Guid id) => Task.FromResult(Room as Room);
            public Task<Room> AddAsync(Room room) => throw new NotImplementedException();
            public Task<IEnumerable<Room>> GetAllAsync() => throw new NotImplementedException();
            public Task SeedRoomsAsync() => Task.CompletedTask;
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsMoviesWithEvents_WhenEventsExist()
        {
            var movieId = new MovieId();
            var movies = new List<EntitiesMovie> { new EntitiesMovie(movieId, "Title", "Desc", "Genre", "Actors", "PG", 120, "url") };
            var events = new List<EntitiesMovieEvent> { new EntitiesMovieEvent { Id = Guid.NewGuid(), MovieId = movieId, RoomId = new RoomId(), Time = DateTime.UtcNow.AddHours(15), Capacity = 10 } };
            var useCase = new FindMoviesWithEventsInTimeframeUseCase(
                new FakeMovieRepository { Movies = movies },
                new FakeMovieEventRepository { Events = events }
            );
            var query = new FindMoviesWithEventsInTimeframeQuery { StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(7) };
            var result = await useCase.ExecuteAsync(query);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Single(result);
            Assert.Equal(movieId, result[0].Id);
            Assert.NotEmpty(result[0].Events);
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsEmptyList_WhenNoEventsFound()
        {
            var movieId = new MovieId();
            var movies = new List<EntitiesMovie> { new EntitiesMovie(movieId, "Title", "Desc", "Genre", "Actors", "PG", 120, "url") };
            var useCase = new FindMoviesWithEventsInTimeframeUseCase(
                new FakeMovieRepository { Movies = movies },
                new FakeMovieEventRepository { Events = new List<EntitiesMovieEvent>() }
            );
            var query = new FindMoviesWithEventsInTimeframeQuery { StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(7) };
            var result = await useCase.ExecuteAsync(query);
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ExecuteAsync_NoMoviesFound_ReturnsEmpty()
        {
            var useCase = new FindMoviesWithEventsInTimeframeUseCase(
                new FakeMovieRepository { Movies = new List<EntitiesMovie>() },
                new FakeMovieEventRepository(),
                new FakeRoomRepository()
            );
            var query = new FindMoviesWithEventsInTimeframeQuery { Title = "None", Genre = "None" };
            var result = await useCase.ExecuteAsync(query);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task ExecuteAsync_MoviesWithNoEvents_ReturnsEmpty()
        {
            var movieId = new MovieId();
            var movie = new EntitiesMovie(movieId, "Title", "Desc", "Genre", "Actors", "PG", 120, "url");
            var movies = new List<EntitiesMovie> { movie };
            var useCase = new FindMoviesWithEventsInTimeframeUseCase(
                new FakeMovieRepository { Movies = movies },
                new FakeMovieEventRepository { Events = new List<EntitiesMovieEvent>() }
            );
            var query = new FindMoviesWithEventsInTimeframeQuery { Title = "Title", Genre = "Genre" };
            var result = await useCase.ExecuteAsync(query);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task ExecuteAsync_NullQuery_ReturnsEmpty()
        {
            var useCase = new FindMoviesWithEventsInTimeframeUseCase(
                new FakeMovieRepository { Movies = new List<EntitiesMovie>() },
                new FakeMovieEventRepository(),
                new FakeRoomRepository()
            );
            var result = await useCase.ExecuteAsync(null);
            Assert.Empty(result.Data);
        }
    }
}
