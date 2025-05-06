using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies.FindMovieEventsForMonth;
using Howestprime.Movies.Domain.Entities;
using Xunit;

namespace UnitTests.Application
{
    public class FindMovieEventsForMonthUseCaseTests
    {
        private class FakeMovieRepository : IMovieRepository
        {
            public Dictionary<Guid, Movie> Movies = new();
            public Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult(Movies.ContainsKey(id) ? Movies[id] : null);
            public Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken = default) => throw new NotImplementedException();
            public Task<IEnumerable<Movie>> FindByFiltersAsync(string? title, string? genre, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        }
        private class FakeMovieEventRepository : IMovieEventRepository
        {
            public IEnumerable<MovieEvent> Events = new List<MovieEvent>();
            public Task<MovieEvent?> GetByRoomDateTimeAsync(Guid roomId, DateTime date, TimeSpan time) => throw new NotImplementedException();
            public Task AddAsync(MovieEvent movieEvent) => throw new NotImplementedException();
            public Task DeleteAsync(Guid id) => throw new NotImplementedException();
            public Task<IEnumerable<MovieEvent>> GetEventsForMovieInRangeAsync(Guid movieId, DateTime start, DateTime end) => throw new NotImplementedException();
            public Task<IEnumerable<MovieEvent>> GetEventsInRangeAsync(DateTime start, DateTime end) => Task.FromResult(Events);
            public Task<MovieEvent> GetByIdWithBookingsAsync(Guid movieEventId) => throw new NotImplementedException();
            public Task UpdateAsync(MovieEvent movieEvent) => throw new NotImplementedException();
        }
        private class FakeRoomRepository : IRoomRepository
        {
            public Dictionary<Guid, Room> Rooms = new();
            public Task<Room?> GetByIdAsync(Guid id) => Task.FromResult(Rooms.ContainsKey(id) ? Rooms[id] : null);
            public Task<Room> AddAsync(Room room) => throw new NotImplementedException();
            public Task<IEnumerable<Room>> GetAllAsync() => throw new NotImplementedException();
            public Task SeedRoomsAsync() => Task.CompletedTask;
        }

        [Fact]
        public async Task ExecuteAsync_InvalidMonthOrYear_ReturnsEmpty()
        {
            var useCase = new FindMovieEventsForMonthUseCase(new FakeMovieRepository(), new FakeMovieEventRepository(), new FakeRoomRepository());
            var query = new FindMovieEventsForMonthQuery { Year = 0, Month = 13 };
            var result = await useCase.ExecuteAsync(query);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ExecuteAsync_NoEvents_ReturnsEmpty()
        {
            var useCase = new FindMovieEventsForMonthUseCase(new FakeMovieRepository(), new FakeMovieEventRepository { Events = new List<MovieEvent>() }, new FakeRoomRepository());
            var query = new FindMovieEventsForMonthQuery { Year = 2025, Month = 5 };
            var result = await useCase.ExecuteAsync(query);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ExecuteAsync_EventsWithMissingMovieOrRoom_SkipsThoseEvents()
        {
            var movieId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var events = new List<MovieEvent>
            {
                new MovieEvent { Id = Guid.NewGuid(), MovieId = movieId, RoomId = roomId, Date = new DateTime(2025, 5, 10), Time = TimeSpan.FromHours(15), Capacity = 10 },
                new MovieEvent { Id = Guid.NewGuid(), MovieId = Guid.NewGuid(), RoomId = roomId, Date = new DateTime(2025, 5, 11), Time = TimeSpan.FromHours(15), Capacity = 10 }, // missing movie
                new MovieEvent { Id = Guid.NewGuid(), MovieId = movieId, RoomId = Guid.NewGuid(), Date = new DateTime(2025, 5, 12), Time = TimeSpan.FromHours(15), Capacity = 10 } // missing room
            };
            var movieRepo = new FakeMovieRepository { Movies = { [movieId] = new Movie(movieId, "Title", "Desc", "Genre", "Actors", "PG", 120, "url") } };
            var eventRepo = new FakeMovieEventRepository { Events = events };
            var roomRepo = new FakeRoomRepository { Rooms = { [roomId] = new Room { Id = roomId, Name = "Room", Capacity = 10 } } };
            var useCase = new FindMovieEventsForMonthUseCase(movieRepo, eventRepo, roomRepo);
            var query = new FindMovieEventsForMonthQuery { Year = 2025, Month = 5 };
            var result = await useCase.ExecuteAsync(query);
            Assert.Single(result);
        }

        [Fact]
        public async Task ExecuteAsync_ValidEvents_ReturnsEventData()
        {
            var movieId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var events = new List<MovieEvent>
            {
                new MovieEvent { Id = Guid.NewGuid(), MovieId = movieId, RoomId = roomId, Date = new DateTime(2025, 5, 10), Time = TimeSpan.FromHours(15), Capacity = 10 }
            };
            var movieRepo = new FakeMovieRepository { Movies = { [movieId] = new Movie(movieId, "Title", "Desc", "Genre", "Actors", "PG", 120, "url") } };
            var eventRepo = new FakeMovieEventRepository { Events = events };
            var roomRepo = new FakeRoomRepository { Rooms = { [roomId] = new Room { Id = roomId, Name = "Room", Capacity = 10 } } };
            var useCase = new FindMovieEventsForMonthUseCase(movieRepo, eventRepo, roomRepo);
            var query = new FindMovieEventsForMonthQuery { Year = 2025, Month = 5 };
            var result = await useCase.ExecuteAsync(query);
            Assert.Single(result);
            Assert.Equal(new DateTime(2025, 5, 10).Date, result[0].Date.ToDateTime(TimeOnly.MinValue).Date);
        }
    }
}
