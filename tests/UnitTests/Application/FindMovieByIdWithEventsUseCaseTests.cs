using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies.FindMovieByIdWithEvents;
using EntitiesMovie = Howestprime.Movies.Domain.Entities.Movie;
using EntitiesMovieEvent = Howestprime.Movies.Domain.Entities.MovieEvent;
using EntitiesRoom = Howestprime.Movies.Domain.Entities.Room;
using Howestprime.Movies.Domain.Entities;
using Howestprime.Movies.Domain.Shared;
using Xunit;

namespace UnitTests.Application
{
    public class FindMovieByIdWithEventsUseCaseTests
    {
        private class FakeMovieRepository : IMovieRepository
        {
            private readonly EntitiesMovie _movie;

            public FakeMovieRepository(EntitiesMovie movie)
            {
                _movie = movie;
            }

            public Task<EntitiesMovie?> GetByIdAsync(MovieId id, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(_movie);
            }

            public Task<Optional<EntitiesMovie>> ById(MovieId id)
            {
                return Task.FromResult(Optional<EntitiesMovie>.Some(_movie));
            }

            public Task Save(EntitiesMovie movie)
            {
                return Task.CompletedTask;
            }

            public Task Remove(EntitiesMovie movie)
            {
                return Task.CompletedTask;
            }

            public Task<IEnumerable<EntitiesMovie>> FindByFiltersAsync(string? title, string? genre, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(new[] { _movie }.AsEnumerable());
            }
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
            private readonly EntitiesRoom _room;

            public FakeRoomRepository(EntitiesRoom room)
            {
                _room = room;
            }

            public Task<EntitiesRoom?> GetByIdAsync(RoomId id)
            {
                return Task.FromResult(_room);
            }

            public Task<IEnumerable<EntitiesRoom>> GetAllAsync()
            {
                return Task.FromResult(new[] { _room }.AsEnumerable());
            }

            public Task<EntitiesRoom> AddAsync(EntitiesRoom room)
            {
                return Task.FromResult(room);
            }

            public Task<Optional<EntitiesRoom>> ById(RoomId id)
            {
                return Task.FromResult(Optional<EntitiesRoom>.Some(_room));
            }

            public Task Save(EntitiesRoom room)
            {
                return Task.CompletedTask;
            }

            public Task Remove(EntitiesRoom room)
            {
                return Task.CompletedTask;
            }

            public Task SeedRoomsAsync()
            {
                return Task.CompletedTask;
            }
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsMovieWithEvents_WhenEventsExist()
        {
            var movieId = new MovieId();
            var roomId = new RoomId();
            var movie = new EntitiesMovie(movieId, "Title", "Desc", 2024, "Genre", "Actors", "PG", 120, "url");
            var room = new EntitiesRoom(roomId, "Room", 10);
            var events = new List<EntitiesMovieEvent> { new EntitiesMovieEvent { Id = Guid.NewGuid(), MovieId = movieId, RoomId = roomId, Time = DateTime.UtcNow.AddHours(15), Capacity = 10 } };
            var useCase = new FindMovieByIdWithEventsUseCase(
                new FakeMovieRepository(movie),
                new FakeMovieEventRepository { Events = events },
                new FakeRoomRepository(room)
            );
            var query = new FindMovieByIdWithEventsQuery { MovieId = movieId };
            var result = await useCase.ExecuteAsync(query);
            Assert.NotNull(result);
            Assert.Equal(movieId, result.Id);
            Assert.Equal("Title", result.Title);
            Assert.Equal("Genre", result.Genres);
            Assert.Equal("Actors", result.Actors);
            Assert.Equal("PG", result.AgeRating);
            Assert.Equal(120, result.Duration);
            Assert.Equal("url", result.PosterUrl);
            Assert.Single(result.Events);
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsException_WhenMovieNotFound()
        {
            var useCase = new FindMovieByIdWithEventsUseCase(
                new FakeMovieRepository(null),
                new FakeMovieEventRepository { Events = new List<EntitiesMovieEvent>() },
                new FakeRoomRepository(null)
            );
            var query = new FindMovieByIdWithEventsQuery { MovieId = new MovieId() };
            await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(query));
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsMovieWithoutEvents_WhenNoEventsExist()
        {
            var movieId = new MovieId();
            var roomId = new RoomId();
            var movie = new EntitiesMovie(movieId, "Title", "Desc", 2024, "Genre", "Actors", "PG", 120, "url");
            var room = new EntitiesRoom(roomId, "Room", 10);
            var useCase = new FindMovieByIdWithEventsUseCase(
                new FakeMovieRepository(movie),
                new FakeMovieEventRepository { Events = new List<EntitiesMovieEvent>() },
                new FakeRoomRepository(room)
            );
            var query = new FindMovieByIdWithEventsQuery { MovieId = movieId };
            var result = await useCase.ExecuteAsync(query);
            Assert.NotNull(result);
            Assert.Equal(movieId, result.Id);
            Assert.Equal("Title", result.Title);
            Assert.Equal("Genre", result.Genres);
            Assert.Equal("Actors", result.Actors);
            Assert.Equal("PG", result.AgeRating);
            Assert.Equal(120, result.Duration);
            Assert.Equal("url", result.PosterUrl);
            Assert.Empty(result.Events);
        }
    }
}
