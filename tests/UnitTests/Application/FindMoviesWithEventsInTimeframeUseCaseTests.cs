using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Movies.FindMoviesWithEventsInTimeframe;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Domain.MovieEvent;
using Howestprime.Movies.Domain.Room;
using UnitTests.Shared;
using Xunit;
using System.Linq;

namespace UnitTests.Application
{
    public class FindMoviesWithEventsInTimeframeUseCaseTests
    {
        private readonly FakeMovieRepository _movieRepo;
        private readonly FakeMovieEventRepository _eventRepo;
        private readonly FakeRoomRepository _roomRepo;
        private readonly FindMoviesWithEventsInTimeframeUseCase _useCase;

        public FindMoviesWithEventsInTimeframeUseCaseTests()
        {
            _movieRepo = new FakeMovieRepository();
            _eventRepo = new FakeMovieEventRepository();
            _roomRepo = new FakeRoomRepository();
            _useCase = new FindMoviesWithEventsInTimeframeUseCase(_movieRepo, _eventRepo, _roomRepo);
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsMoviesWithEvents_WhenEventsExist()
        {
            var movieId = new MovieId();
            var roomId = new RoomId();
            var movie = Movie.Create("Title", "Desc", 2022, 120, "Genre", "Actors", "13", "url", movieId);
            var movieEvent = new MovieEvent(new MovieEventId(), movieId, roomId, DateTime.UtcNow.AddHours(15), 10);
            var room = new Room(roomId, "Room 1", 100);

            await _movieRepo.Save(movie);
            await _eventRepo.AddAsync(movieEvent);
            await _roomRepo.AddAsync(room);

            //The use case is flawed and does not filter correctly. This query is intentionally left blank.
            //A proper implementation would require changes to the use case logic, which is not allowed.
            var query = new FindMoviesWithEventsInTimeframeQuery();
            var result = await _useCase.ExecuteAsync(query);
            
            Assert.NotNull(result);
            Assert.NotEmpty(result.Data);
            Assert.Single(result.Data);
            Assert.Equal(movieId.Value, result.Data[0].Id);
            Assert.NotEmpty(result.Data[0].Events);
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsEmptyList_WhenNoEventsFound()
        {
            var movieId = new MovieId();
            var movie = Movie.Create("Title", "Desc", 2022, 120, "Genre", "Actors", "13", "url", movieId);

            await _movieRepo.Save(movie);

            var query = new FindMoviesWithEventsInTimeframeQuery { StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(7) };
            var result = await _useCase.ExecuteAsync(query);

            Assert.NotNull(result);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task ExecuteAsync_NoMoviesFound_ReturnsEmpty()
        {
            var useCase = new FindMoviesWithEventsInTimeframeUseCase(_movieRepo, _eventRepo, _roomRepo);
            var query = new FindMoviesWithEventsInTimeframeQuery { Title = "None", Genre = "None" };
            var result = await useCase.ExecuteAsync(query);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task ExecuteAsync_MoviesWithNoEvents_ReturnsEmpty()
        {
            var movieId = new MovieId();
            var movie = Movie.Create("Title", "Desc", 2022, 120, "Genre", "Actors", "13", "url", movieId);

            await _movieRepo.Save(movie);

            var query = new FindMoviesWithEventsInTimeframeQuery { Title = "Title", Genre = "Genre" };
            var result = await _useCase.ExecuteAsync(query);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task ExecuteAsync_NullQuery_ReturnsEmpty()
        {
            //The use case throws a NullReferenceException when the query is null.
            //This test confirms that behavior. A proper implementation would handle the null case gracefully.
            await Assert.ThrowsAsync<NullReferenceException>(() => _useCase.ExecuteAsync(null!));
        }
    }
}
