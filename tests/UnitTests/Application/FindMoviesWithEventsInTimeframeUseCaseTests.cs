using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies.FindMoviesWithEventsInTimeframe;
using Howestprime.Movies.Domain.Entities;
using Xunit;

namespace UnitTests.Application
{
    public class FindMoviesWithEventsInTimeframeUseCaseTests
    {
        private class FakeMovieRepository : IMovieRepository
        {
            public IEnumerable<Movie> Movies = new List<Movie>();
            public Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult((Movie?)null);
            public Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken = default) => throw new NotImplementedException();
            public Task<IEnumerable<Movie>> FindByFiltersAsync(string? title, string? genre, CancellationToken cancellationToken = default) => Task.FromResult(Movies);
        }
        private class FakeMovieEventRepository : IMovieEventRepository
        {
            public Dictionary<Guid, IEnumerable<MovieEvent>> EventsByMovie = new();
            public Task<MovieEvent?> GetByRoomDateTimeAsync(Guid roomId, DateTime date, TimeSpan time) => throw new NotImplementedException();
            public Task AddAsync(MovieEvent movieEvent) => throw new NotImplementedException();
            public Task DeleteAsync(Guid id) => throw new NotImplementedException();
            public Task<IEnumerable<MovieEvent>> GetEventsForMovieInRangeAsync(Guid movieId, DateTime start, DateTime end) =>
                Task.FromResult(EventsByMovie.ContainsKey(movieId) ? EventsByMovie[movieId] : Enumerable.Empty<MovieEvent>());
            public Task<IEnumerable<MovieEvent>> GetEventsInRangeAsync(DateTime start, DateTime end) => throw new NotImplementedException();
            public Task<MovieEvent> GetByIdWithBookingsAsync(Guid movieEventId) => throw new NotImplementedException();
            public Task UpdateAsync(MovieEvent movieEvent) => throw new NotImplementedException();
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
        public async Task ExecuteAsync_ReturnsMoviesWithEvents()
        {
            var movieId = Guid.NewGuid();
            var movie = new Movie(movieId, "Title", "Desc", "Genre", "Actors", "PG", 120, "url");
            var movies = new List<Movie> { movie };
            var events = new List<MovieEvent> { new MovieEvent { Id = Guid.NewGuid(), MovieId = movieId, RoomId = Guid.NewGuid(), Time = DateTime.UtcNow.AddHours(15), Capacity = 10 } };
            var useCase = new FindMoviesWithEventsInTimeframeUseCase(
                new FakeMovieRepository { Movies = movies },
                new FakeMovieEventRepository { EventsByMovie = { [movieId] = events } },
                new FakeRoomRepository()
            );
            var query = new FindMoviesWithEventsInTimeframeQuery { Title = "Title", Genre = "Genre" };
            var result = await useCase.ExecuteAsync(query);
            Assert.NotEmpty(result.Data);
            Assert.NotEmpty(result.Data.First().Events);
        }

        [Fact]
        public async Task ExecuteAsync_NoMoviesFound_ReturnsEmpty()
        {
            var useCase = new FindMoviesWithEventsInTimeframeUseCase(
                new FakeMovieRepository { Movies = new List<Movie>() },
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
            var movieId = Guid.NewGuid();
            var movie = new Movie(movieId, "Title", "Desc", "Genre", "Actors", "PG", 120, "url");
            var movies = new List<Movie> { movie };
            var useCase = new FindMoviesWithEventsInTimeframeUseCase(
                new FakeMovieRepository { Movies = movies },
                new FakeMovieEventRepository { EventsByMovie = { [movieId] = new List<MovieEvent>() } },
                new FakeRoomRepository()
            );
            var query = new FindMoviesWithEventsInTimeframeQuery { Title = "Title", Genre = "Genre" };
            var result = await useCase.ExecuteAsync(query);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task ExecuteAsync_NullQuery_ReturnsEmpty()
        {
            var useCase = new FindMoviesWithEventsInTimeframeUseCase(
                new FakeMovieRepository { Movies = new List<Movie>() },
                new FakeMovieEventRepository(),
                new FakeRoomRepository()
            );
            var result = await useCase.ExecuteAsync(null);
            Assert.Empty(result.Data);
        }
    }
}
