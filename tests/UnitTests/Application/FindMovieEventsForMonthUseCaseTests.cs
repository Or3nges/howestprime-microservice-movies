using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Howestprime.Movies.Application.Movies.FindMovieEventsForMonth;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Domain.MovieEvent;
using Howestprime.Movies.Domain.Room;
using UnitTests.Shared;
using Howestprime.Movies.Application.Contracts.Data;
using System.Linq;

namespace UnitTests.Application
{
    public class FindMovieEventsForMonthUseCaseTests
    {
        private readonly FakeMovieRepository _movieRepository;
        private readonly FakeRoomRepository _roomRepository;
        private readonly FakeMovieEventRepository _movieEventRepository;
        private readonly FindMovieEventsForMonthUseCase _useCase;

        public FindMovieEventsForMonthUseCaseTests()
        {
            _movieRepository = new FakeMovieRepository();
            _roomRepository = new FakeRoomRepository();
            _movieEventRepository = new FakeMovieEventRepository();
            _useCase = new FindMovieEventsForMonthUseCase(
                _movieRepository,
                _movieEventRepository,
                _roomRepository
            );
        }

        [Fact]
        public async Task ExecuteAsync_WithEventsInMonth_ReturnsEvents()
        {
            // Arrange
            var movieId = new MovieId();
            var roomId = new RoomId();
            var eventId = new MovieEventId();

            var movie = Movie.Create(
                "Test Movie",
                "Description",
                2024,
                120,
                "Action",
                "Actor 1, Actor 2",
                "12",
                "poster.jpg",
                movieId
            );

            var room = new Room(roomId, "Room 1", 100);

            var now = DateTime.UtcNow;
            var eventTime = new DateTime(now.Year, now.Month, 15, 14, 30, 0, DateTimeKind.Utc).AddMonths(1);
            var movieEvent = new MovieEvent(eventId, movieId, roomId, eventTime, 100);

            await _movieRepository.Save(movie);
            await _roomRepository.AddAsync(room);
            await _movieEventRepository.AddAsync(movieEvent);

            var query = new FindMovieEventsForMonthQuery { Year = eventTime.Year, Month = eventTime.Month };

            // Act
            var result = await _useCase.ExecuteAsync(query);

            // Assert
            var eventData = Assert.Single(result);
            Assert.Equal(eventId.Value, eventData.Id);
            Assert.Equal(movieId.Value, eventData.MovieId);
            Assert.Equal(eventTime, eventData.Time);
            Assert.Equal(100, eventData.Capacity);
            Assert.NotNull(eventData.Room);
            Assert.Equal(roomId.Value, eventData.Room.Id);
            Assert.Equal("Room 1", eventData.Room.Name);
            Assert.Equal(100, eventData.Room.Capacity);
        }

        [Fact]
        public async Task ExecuteAsync_WithNoEventsInMonth_ReturnsEmptyList()
        {
            // Arrange
            var movieId = new MovieId();
            var roomId = new RoomId();
            var eventId = new MovieEventId();

            var movie = Movie.Create(
                "Test Movie",
                "Description",
                2024,
                120,
                "Action",
                "Actor 1, Actor 2",
                "12",
                "poster.jpg",
                movieId
            );

            var room = new Room(roomId, "Room 1", 100);

            var now = DateTime.UtcNow;
            var eventTime = new DateTime(now.Year, now.Month, 15, 14, 30, 0, DateTimeKind.Utc).AddMonths(1);
            var movieEvent = new MovieEvent(eventId, movieId, roomId, eventTime, 100);

            await _movieRepository.Save(movie);
            await _roomRepository.AddAsync(room);
            await _movieEventRepository.AddAsync(movieEvent);

            var query = new FindMovieEventsForMonthQuery { Year = eventTime.Year, Month = eventTime.Month + 1 };

            // Act
            var result = await _useCase.ExecuteAsync(query);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task ExecuteAsync_WithMultipleEventsInMonth_ReturnsAllEvents()
        {
            // Arrange
            var movieId = new MovieId();
            var roomId = new RoomId();
            var eventId1 = new MovieEventId();
            var eventId2 = new MovieEventId();

            var movie = Movie.Create(
                "Test Movie",
                "Description",
                2024,
                120,
                "Action",
                "Actor 1, Actor 2",
                "12",
                "poster.jpg",
                movieId
            );

            var room = new Room(roomId, "Room 1", 100);

            var now = DateTime.UtcNow;
            var eventTime1 = new DateTime(now.Year, now.Month, 15, 14, 30, 0, DateTimeKind.Utc).AddMonths(1);
            var eventTime2 = new DateTime(now.Year, now.Month, 16, 14, 30, 0, DateTimeKind.Utc).AddMonths(1);
            var movieEvent1 = new MovieEvent(eventId1, movieId, roomId, eventTime1, 100);
            var movieEvent2 = new MovieEvent(eventId2, movieId, roomId, eventTime2, 100);

            await _movieRepository.Save(movie);
            await _roomRepository.AddAsync(room);
            await _movieEventRepository.AddAsync(movieEvent1);
            await _movieEventRepository.AddAsync(movieEvent2);

            var query = new FindMovieEventsForMonthQuery { Year = eventTime1.Year, Month = eventTime1.Month };

            // Act
            var result = await _useCase.ExecuteAsync(query);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, e => e.Id == eventId1.Value);
            Assert.Contains(result, e => e.Id == eventId2.Value);
        }

        [Fact]
        public async Task ExecuteAsync_WithDeletedMovie_SkipsEvent()
        {
            // Arrange
            var movieId = new MovieId();
            var roomId = new RoomId();
            var eventId = new MovieEventId();

            var room = new Room(roomId, "Room 1", 100);

            var now = DateTime.UtcNow;
            var eventTime = new DateTime(now.Year, now.Month, 15, 14, 30, 0, DateTimeKind.Utc).AddMonths(1);
            var movieEvent = new MovieEvent(eventId, movieId, roomId, eventTime, 100);

            await _roomRepository.AddAsync(room);
            await _movieEventRepository.AddAsync(movieEvent);

            var query = new FindMovieEventsForMonthQuery { Year = eventTime.Year, Month = eventTime.Month };

            // Act
            var result = await _useCase.ExecuteAsync(query);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task ExecuteAsync_WithDeletedRoom_SkipsEvent()
        {
            // Arrange
            var movieId = new MovieId();
            var roomId = new RoomId();
            var eventId = new MovieEventId();

            var movie = Movie.Create(
                "Test Movie",
                "Description",
                2024,
                120,
                "Action",
                "Actor 1, Actor 2",
                "12",
                "poster.jpg",
                movieId
            );

            var now = DateTime.UtcNow;
            var eventTime = new DateTime(now.Year, now.Month, 15, 14, 30, 0, DateTimeKind.Utc).AddMonths(1);
            var movieEvent = new MovieEvent(eventId, movieId, roomId, eventTime, 100);

            await _movieRepository.Save(movie);
            await _movieEventRepository.AddAsync(movieEvent);

            var query = new FindMovieEventsForMonthQuery { Year = eventTime.Year, Month = eventTime.Month };

            // Act
            var result = await _useCase.ExecuteAsync(query);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task ExecuteAsync_WithEventsAtMonthBoundaries_ReturnsOnlyEventsInMonth()
        {
            // Arrange
            var movieId = new MovieId();
            var roomId = new RoomId();
            var eventId1 = new MovieEventId();
            var eventId2 = new MovieEventId();
            var eventId3 = new MovieEventId();

            var movie = Movie.Create(
                "Test Movie",
                "Description",
                2024,
                120,
                "Action",
                "Actor 1, Actor 2",
                "12",
                "poster.jpg",
                movieId
            );

            var room = new Room(roomId, "Room 1", 100);
            
            var aTimeInTheFuture = DateTime.UtcNow.AddMonths(2);
            var startOfTargetMonth = new DateTime(aTimeInTheFuture.Year, aTimeInTheFuture.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            var eventTimeBefore = startOfTargetMonth.AddSeconds(-1);
            var eventTimeDuring = startOfTargetMonth.AddHours(1);
            var eventTimeAfter = startOfTargetMonth.AddMonths(1);

            var movieEventBefore = new MovieEvent(eventId1, movieId, roomId, eventTimeBefore, 100);
            var movieEventDuring = new MovieEvent(eventId2, movieId, roomId, eventTimeDuring, 100);
            var movieEventAfter = new MovieEvent(eventId3, movieId, roomId, eventTimeAfter, 100);

            await _movieRepository.Save(movie);
            await _roomRepository.AddAsync(room);
            await _movieEventRepository.AddAsync(movieEventBefore);
            await _movieEventRepository.AddAsync(movieEventDuring);
            await _movieEventRepository.AddAsync(movieEventAfter);

            var query = new FindMovieEventsForMonthQuery { Year = startOfTargetMonth.Year, Month = startOfTargetMonth.Month };

            // Act
            var result = await _useCase.ExecuteAsync(query);

            // Assert
            var eventData = Assert.Single(result);
            Assert.Equal(eventId2.Value, eventData.Id);
        }
    }
}
