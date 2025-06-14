using System;
using System.Threading.Tasks;
using Xunit;
using Howestprime.Movies.Application.Movies.FindMovieByIdWithEvents;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Domain.MovieEvent;
using Howestprime.Movies.Domain.Room;
using UnitTests.Shared;
using Howestprime.Movies.Application.Contracts.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;

namespace UnitTests.Application
{
    public class FindMovieByIdWithEventsUseCaseTests
    {
        private readonly FakeMovieRepository _movieRepository;
        private readonly FakeMovieEventRepository _movieEventRepository;
        private readonly FakeRoomRepository _roomRepository;
        private readonly FindMovieByIdWithEventsUseCase _useCase;

        public FindMovieByIdWithEventsUseCaseTests()
        {
            _movieRepository = new FakeMovieRepository();
            _movieEventRepository = new FakeMovieEventRepository();
            _roomRepository = new FakeRoomRepository();
            _useCase = new FindMovieByIdWithEventsUseCase(_movieRepository, _movieEventRepository, _roomRepository);
        }

        [Fact]
        public async Task ExecuteAsync_WithValidId_ReturnsMovieWithEvents()
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

            var eventTime = DateTime.UtcNow.AddDays(1);
            var movieEvent = new MovieEvent(eventId, movieId, roomId, eventTime, 50);

            await _movieRepository.Save(movie);
            await _roomRepository.AddAsync(room);
            await _movieEventRepository.AddAsync(movieEvent);

            var query = new FindMovieByIdWithEventsQuery { MovieId = movieId };

            // Act
            var result = await _useCase.ExecuteAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(movieId.Value, result.Id);
            Assert.Equal(movie.Title, result.Title);
            Assert.Equal(movie.Description, result.Description);
            Assert.Equal(movie.Year, result.Year);
            Assert.Equal(movie.Genre, result.Genre);
            Assert.Equal(movie.Actors, result.Actors);
            Assert.Equal(int.Parse(movie.AgeRating), result.AgeRating);
            Assert.Equal(movie.Duration, result.Duration);
            Assert.Equal(movie.PosterUrl, result.PosterUrl);

            var eventData = Assert.Single(result.Events);
            Assert.Equal(eventId.Value, eventData.Id);
            Assert.Equal(movieId.Value, eventData.MovieId);
            Assert.Equal(eventTime, eventData.Time);
            Assert.Equal(50, eventData.Capacity);
            Assert.NotNull(eventData.Room);
            Assert.Equal(roomId.Value, eventData.Room.Id);
            Assert.Equal("Room 1", eventData.Room.Name);
            Assert.Equal(100, eventData.Room.Capacity);
        }

        [Fact]
        public async Task ExecuteAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            var query = new FindMovieByIdWithEventsQuery { MovieId = new MovieId(Guid.NewGuid().ToString()) };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(query));
        }

        [Fact]
        public async Task ExecuteAsync_WithNoEvents_ReturnsMovieWithEmptyEvents()
        {
            // Arrange
            var movieId = new MovieId();

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

            await _movieRepository.Save(movie);

            var query = new FindMovieByIdWithEventsQuery { MovieId = movieId };

            // Act
            var result = await _useCase.ExecuteAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(movieId.Value, result.Id);
            Assert.Empty(result.Events);
        }

        [Fact]
        public async Task ExecuteAsync_WithMultipleEvents_ReturnsAllEvents()
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

            var eventTime1 = DateTime.UtcNow.AddDays(1);
            var eventTime2 = DateTime.UtcNow.AddDays(2);
            var movieEvent1 = new MovieEvent(eventId1, movieId, roomId, eventTime1, 50);
            var movieEvent2 = new MovieEvent(eventId2, movieId, roomId, eventTime2, 50);

            await _movieRepository.Save(movie);
            await _roomRepository.AddAsync(room);
            await _movieEventRepository.AddAsync(movieEvent1);
            await _movieEventRepository.AddAsync(movieEvent2);

            var query = new FindMovieByIdWithEventsQuery { MovieId = movieId };

            // Act
            var result = await _useCase.ExecuteAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Events.Count);
            Assert.Contains(result.Events, e => e.Id == eventId1.Value);
            Assert.Contains(result.Events, e => e.Id == eventId2.Value);
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

            var eventTime = DateTime.UtcNow.AddDays(1);
            var movieEvent = new MovieEvent(eventId, movieId, roomId, eventTime, 50);

            await _movieRepository.Save(movie);
            await _movieEventRepository.AddAsync(movieEvent);

            var query = new FindMovieByIdWithEventsQuery { MovieId = movieId };

            // Act
            var result = await _useCase.ExecuteAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Events);
        }

        [Fact]
        public async Task ExecuteAsync_WithPastEvents_FiltersOutPastEvents()
        {
            // Arrange
            var movieId = new MovieId();
            var roomId = new RoomId();
            var pastEventId = new MovieEventId();
            var futureEventId = new MovieEventId();

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

            var pastEventTime = DateTime.UtcNow.AddDays(-1);
            var futureEventTime = DateTime.UtcNow.AddDays(1);

            var pastEvent = CreatePastMovieEvent(pastEventId, movieId, roomId, pastEventTime, 50);
            var futureEvent = new MovieEvent(futureEventId, movieId, roomId, futureEventTime, 50);

            await _movieRepository.Save(movie);
            await _roomRepository.AddAsync(room);
            await _movieEventRepository.AddAsync(pastEvent);
            await _movieEventRepository.AddAsync(futureEvent);

            var query = new FindMovieByIdWithEventsQuery { MovieId = movieId };

            // Act
            var result = await _useCase.ExecuteAsync(query);

            // Assert
            Assert.NotNull(result);
            var singleEvent = Assert.Single(result.Events);
            Assert.Equal(futureEventId.Value, singleEvent.Id);
        }
        
        private MovieEvent CreatePastMovieEvent(MovieEventId id, MovieId movieId, RoomId roomId, DateTime time, int capacity)
        {
            var movieEvent = (MovieEvent)FormatterServices.GetUninitializedObject(typeof(MovieEvent));
            
            var idField = typeof(MovieEvent).BaseType.GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            idField.SetValue(movieEvent, id);

            var movieIdField = typeof(MovieEvent).GetField("<MovieId>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            movieIdField.SetValue(movieEvent, movieId);
            
            var roomIdField = typeof(MovieEvent).GetField("<RoomId>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            roomIdField.SetValue(movieEvent, roomId);
            
            var timeField = typeof(MovieEvent).GetField("<Time>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            timeField.SetValue(movieEvent, time);
            
            var capacityField = typeof(MovieEvent).GetField("<Capacity>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            capacityField.SetValue(movieEvent, capacity);
            
            var bookingsField = typeof(MovieEvent).GetField("<Bookings>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            bookingsField.SetValue(movieEvent, new System.Collections.Generic.List<Howestprime.Movies.Domain.Booking.Booking>());

            return movieEvent;
        }
    }
}
