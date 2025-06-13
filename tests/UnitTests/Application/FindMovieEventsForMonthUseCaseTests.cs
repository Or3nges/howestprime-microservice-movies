using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies.FindMovieEventsForMonth;
using EntitiesMovie = Howestprime.Movies.Domain.Entities.Movie;
using EntitiesMovieEvent = Howestprime.Movies.Domain.Entities.MovieEvent;
using EntitiesRoom = Howestprime.Movies.Domain.Entities.Room;
using SharedMovieEventData = Howestprime.Movies.Domain.Shared.MovieEventData;
using Howestprime.Movies.Domain.Shared;
using Xunit;

namespace UnitTests.Application
{
    public class FindMovieEventsForMonthUseCaseTests
    {
        private class FakeMovieRepository : IMovieRepository
        {
            public EntitiesMovie? Movie;
            public Task<EntitiesMovie?> GetByIdAsync(MovieId id, CancellationToken cancellationToken = default) => Task.FromResult(Movie);
            public Task<EntitiesMovie> AddAsync(EntitiesMovie movie, CancellationToken cancellationToken = default) => throw new NotImplementedException();
            public Task<IEnumerable<EntitiesMovie>> FindByFiltersAsync(string? title, string? genre, CancellationToken cancellationToken = default) => throw new NotImplementedException();
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
            public Dictionary<Guid, EntitiesRoom> Rooms = new();
            public Task<EntitiesRoom?> GetByIdAsync(Guid id) => Task.FromResult(Rooms.ContainsKey(id) ? Rooms[id] : null);
            public Task<EntitiesRoom> AddAsync(EntitiesRoom room) => throw new NotImplementedException();
            public Task<IEnumerable<EntitiesRoom>> GetAllAsync() => throw new NotImplementedException();
            public Task SeedRoomsAsync() => Task.CompletedTask;
        }

        [Fact]
        public async Task ExecuteAsync_InvalidMonthOrYear_ReturnsEmpty()
        {
            var useCase = new FindMovieEventsForMonthUseCase(new FakeMovieRepository(), new FakeMovieEventRepository(), new FakeRoomRepository());
            var query = new FindMovieEventsForMonthQuery { Year = 0, Month = 13 };
            var result = await useCase.ExecuteAsync(query);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task ExecuteAsync_NoEvents_ReturnsEmpty()
        {
            var useCase = new FindMovieEventsForMonthUseCase(new FakeMovieRepository(), new FakeMovieEventRepository { Events = new List<EntitiesMovieEvent>() }, new FakeRoomRepository());
            var query = new FindMovieEventsForMonthQuery { Year = 2025, Month = 5 };
            var result = await useCase.ExecuteAsync(query);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task ExecuteAsync_EventsWithMissingMovieOrRoom_SkipsThoseEvents()
        {
            var movieId = new MovieId();
            var roomId = new RoomId();
            var events = new List<EntitiesMovieEvent>
            {
                new EntitiesMovieEvent { Id = Guid.NewGuid(), MovieId = movieId, RoomId = roomId, Time = new DateTime(2025, 5, 10, 15, 0, 0, DateTimeKind.Utc), Capacity = 10 },
                new EntitiesMovieEvent { Id = Guid.NewGuid(), MovieId = Guid.NewGuid(), RoomId = roomId, Time = new DateTime(2025, 5, 11, 15, 0, 0, DateTimeKind.Utc), Capacity = 10 }, // missing movie
                new EntitiesMovieEvent { Id = Guid.NewGuid(), MovieId = movieId, RoomId = Guid.NewGuid(), Time = new DateTime(2025, 5, 12, 15, 0, 0, DateTimeKind.Utc), Capacity = 10 } // missing room
            };
            var movieRepo = new FakeMovieRepository { Movie = new EntitiesMovie(movieId, "Title", "Desc", "Genre", "Actors", "PG", 120, "url") };
            var eventRepo = new FakeMovieEventRepository { Events = events };
            var roomRepo = new FakeRoomRepository { Rooms = { [roomId.Value] = new EntitiesRoom { Id = roomId.Value, Name = "Room", Capacity = 10 } } };
            var useCase = new FindMovieEventsForMonthUseCase(movieRepo, eventRepo, roomRepo);
            var query = new FindMovieEventsForMonthQuery { Year = 2025, Month = 5 };
            var result = await useCase.ExecuteAsync(query);
            Assert.Single(result.Data);
        }

        [Fact]
        public async Task ExecuteAsync_EventsWithNullRoomOrMovie_AreSkipped()
        {
            var movieId = new MovieId();
            var roomId = new RoomId();
            var events = new List<EntitiesMovieEvent>
            {
                new EntitiesMovieEvent { Id = Guid.NewGuid(), MovieId = movieId, RoomId = roomId, Time = new DateTime(2025, 5, 10, 15, 0, 0, DateTimeKind.Utc), Capacity = 10 },
                new EntitiesMovieEvent { Id = Guid.NewGuid(), MovieId = Guid.NewGuid(), RoomId = roomId, Time = new DateTime(2025, 5, 11, 15, 0, 0, DateTimeKind.Utc), Capacity = 10 }, // missing movie
                new EntitiesMovieEvent { Id = Guid.NewGuid(), MovieId = movieId, RoomId = Guid.NewGuid(), Time = new DateTime(2025, 5, 12, 15, 0, 0, DateTimeKind.Utc), Capacity = 10 } // missing room
            };
            var movieRepo = new FakeMovieRepository { Movie = new EntitiesMovie(movieId, "Title", "Desc", "Genre", "Actors", "PG", 120, "url") };
            var eventRepo = new FakeMovieEventRepository { Events = events };
            var roomRepo = new FakeRoomRepository { Rooms = { [roomId.Value] = new EntitiesRoom { Id = roomId.Value, Name = "Room", Capacity = 10 } } };
            var useCase = new FindMovieEventsForMonthUseCase(movieRepo, eventRepo, roomRepo);
            var query = new FindMovieEventsForMonthQuery { Year = 2025, Month = 5 };
            var result = await useCase.ExecuteAsync(query);
            Assert.Single(result.Data);
        }

        [Fact]
        public async Task ExecuteAsync_ValidEvents_ReturnsEventData()
        {
            var movieId = new MovieId();
            var roomId = new RoomId();
            var events = new List<EntitiesMovieEvent>
            {
                new EntitiesMovieEvent { Id = Guid.NewGuid(), MovieId = movieId, RoomId = roomId, Time = new DateTime(2025, 5, 10, 15, 0, 0, DateTimeKind.Utc), Capacity = 10 }
            };
            var movieRepo = new FakeMovieRepository { Movie = new EntitiesMovie(movieId, "Title", "Desc", "Genre", "Actors", "PG", 120, "url") };
            var eventRepo = new FakeMovieEventRepository { Events = events };
            var roomRepo = new FakeRoomRepository { Rooms = { [roomId.Value] = new EntitiesRoom { Id = roomId.Value, Name = "Room", Capacity = 10 } } };
            var useCase = new FindMovieEventsForMonthUseCase(movieRepo, eventRepo, roomRepo);
            var query = new FindMovieEventsForMonthQuery { Year = 2025, Month = 5 };
            var result = await useCase.ExecuteAsync(query);
            Assert.Single(result.Data);
            Assert.Equal(movieId, result.Data.First().MovieId);
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsEvents_WhenMovieAndEventsExist()
        {
            var movieId = new MovieId();
            var movie = new EntitiesMovie(movieId, "Title", "Desc", "Genre", "Actors", "PG", 120, "url");
            var events = new List<EntitiesMovieEvent> { new EntitiesMovieEvent { Id = Guid.NewGuid(), MovieId = movieId, RoomId = new RoomId(), Time = DateTime.UtcNow.AddHours(15), Capacity = 10 } };
            var useCase = new FindMovieEventsForMonthUseCase(
                new FakeMovieRepository { Movie = movie },
                new FakeMovieEventRepository { Events = events }
            );
            var query = new FindMovieEventsForMonthQuery { MovieId = movieId, Month = DateTime.UtcNow.Month, Year = DateTime.UtcNow.Year };
            var result = await useCase.ExecuteAsync(query);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Single(result);
            Assert.Equal(movieId, result[0].MovieId);
        }

        [Fact]
        public async Task ExecuteAsync_Throws_WhenMovieNotFound()
        {
            var useCase = new FindMovieEventsForMonthUseCase(
                new FakeMovieRepository { Movie = null },
                new FakeMovieEventRepository { Events = new List<EntitiesMovieEvent>() }
            );
            var query = new FindMovieEventsForMonthQuery { MovieId = new MovieId(), Month = DateTime.UtcNow.Month, Year = DateTime.UtcNow.Year };
            await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(query));
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsEmptyList_WhenNoEventsFound()
        {
            var movieId = new MovieId();
            var movie = new EntitiesMovie(movieId, "Title", "Desc", "Genre", "Actors", "PG", 120, "url");
            var useCase = new FindMovieEventsForMonthUseCase(
                new FakeMovieRepository { Movie = movie },
                new FakeMovieEventRepository { Events = new List<EntitiesMovieEvent>() }
            );
            var query = new FindMovieEventsForMonthQuery { MovieId = movieId, Month = DateTime.UtcNow.Month, Year = DateTime.UtcNow.Year };
            var result = await useCase.ExecuteAsync(query);
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
