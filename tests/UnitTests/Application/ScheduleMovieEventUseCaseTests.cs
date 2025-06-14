using System;
using System.Threading.Tasks;
using Xunit;
using Howestprime.Movies.Application.Movies.ScheduleMovieEvent;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Domain.Room;
using Howestprime.Movies.Domain.MovieEvent;
using UnitTests.Shared;
using Howestprime.Movies.Application.Contracts.Ports;

namespace UnitTests.Application
{
    public class ScheduleMovieEventUseCaseTests
    {
        private readonly FakeMovieRepository _movieRepository;
        private readonly FakeRoomRepository _roomRepository;
        private readonly FakeMovieEventRepository _movieEventRepository;
        private readonly FakeUnitOfWork _unitOfWork;
        private readonly ScheduleMovieEventUseCase _useCase;

        public ScheduleMovieEventUseCaseTests()
        {
            _movieRepository = new FakeMovieRepository();
            _roomRepository = new FakeRoomRepository();
            _movieEventRepository = new FakeMovieEventRepository();
            _unitOfWork = new FakeUnitOfWork();
            _useCase = new ScheduleMovieEventUseCase(
                _movieRepository,
                _roomRepository,
                _movieEventRepository,
                _unitOfWork
            );
        }

        [Fact]
        public async Task ExecuteAsync_WithValidCommand_SchedulesEvent()
        {
            // Arrange
            var movieId = new MovieId();
            var roomId = new RoomId();
            var startDate = DateTime.UtcNow.AddDays(1);

            var movie = Movie.Create(
                "Test Movie",
                "Description",
                DateTime.UtcNow.Year,
                120,
                "Action",
                "Actor 1, Actor 2",
                "12",
                "poster.jpg",
                movieId
            );

            var room = new Room(roomId, "Room 1", 100);

            await _movieRepository.Save(movie);
            await _roomRepository.AddAsync(room);

            var command = new ScheduleMovieEventCommand
            {
                MovieId = movieId.Value,
                RoomId = roomId.Value,
                StartDate = startDate,
                Capacity = 100,
                Visitors = 0
            };

            // Act
            await _useCase.ExecuteAsync(command);

            // Assert
            Assert.True(_unitOfWork.Committed);
            var events = await _movieEventRepository.GetEventsForMovieInRangeAsync(movieId, startDate.AddDays(-1), startDate.AddDays(1));
            var movieEvent = Assert.Single(events);
            Assert.Equal(movieId, movieEvent.MovieId);
            Assert.Equal(roomId, movieEvent.RoomId);
            Assert.Equal(startDate, movieEvent.Time);
            Assert.Equal(100, movieEvent.Capacity);
        }

        [Fact]
        public async Task ExecuteAsync_WithNonExistentMovie_ThrowsException()
        {
            // Arrange
            var roomId = new RoomId();
            var room = new Room(roomId, "Room 1", 100);
            await _roomRepository.AddAsync(room);

            var command = new ScheduleMovieEventCommand
            {
                MovieId = Guid.NewGuid().ToString(),
                RoomId = roomId.Value,
                StartDate = DateTime.UtcNow.AddDays(1),
                Capacity = 100,
                Visitors = 0
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.ExecuteAsync(command));
            Assert.Contains("Movie with ID", exception.Message);
            Assert.False(_unitOfWork.Committed);
        }

        [Fact]
        public async Task ExecuteAsync_WithNonExistentRoom_ThrowsException()
        {
            // Arrange
            var movieId = new MovieId();
            var movie = Movie.Create(
                "Test Movie",
                "Description",
                DateTime.UtcNow.Year,
                120,
                "Action",
                "Actor 1, Actor 2",
                "12",
                "poster.jpg",
                movieId
            );
            await _movieRepository.Save(movie);

            var command = new ScheduleMovieEventCommand
            {
                MovieId = movieId.Value,
                RoomId = Guid.NewGuid().ToString(),
                StartDate = DateTime.UtcNow.AddDays(1),
                Capacity = 100,
                Visitors = 0
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.ExecuteAsync(command));
            Assert.Contains("Room with ID", exception.Message);
            Assert.False(_unitOfWork.Committed);
        }

        [Fact]
        public async Task ExecuteAsync_WithExistingEventAtSameTime_ThrowsException()
        {
            // Arrange
            var movieId = new MovieId();
            var roomId = new RoomId();
            var startDate = DateTime.UtcNow.AddDays(1);

            var movie = Movie.Create(
                "Test Movie",
                "Description",
                DateTime.UtcNow.Year,
                120,
                "Action",
                "Actor 1, Actor 2",
                "12",
                "poster.jpg",
                movieId
            );

            var room = new Room(roomId, "Room 1", 100);

            await _movieRepository.Save(movie);
            await _roomRepository.AddAsync(room);

            var existingEvent = new MovieEvent(
                new MovieEventId(),
                movieId,
                roomId,
                startDate,
                100
            );
            await _movieEventRepository.AddAsync(existingEvent);

            var command = new ScheduleMovieEventCommand
            {
                MovieId = movieId.Value,
                RoomId = roomId.Value,
                StartDate = startDate,
                Capacity = 100,
                Visitors = 0
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(command));
            Assert.Contains("already exists", exception.Message);
            Assert.False(_unitOfWork.Committed);
        }

        [Fact]
        public async Task ExecuteAsync_WithPastDate_ThrowsException()
        {
            // Arrange
            var movieId = new MovieId();
            var roomId = new RoomId();
            var startDate = DateTime.UtcNow.AddDays(-1);

            var movie = Movie.Create(
                "Test Movie",
                "Description",
                DateTime.UtcNow.Year,
                120,
                "Action",
                "Actor 1, Actor 2",
                "12",
                "poster.jpg",
                movieId
            );

            var room = new Room(roomId, "Room 1", 100);

            await _movieRepository.Save(movie);
            await _roomRepository.AddAsync(room);

            var command = new ScheduleMovieEventCommand
            {
                MovieId = movieId.Value,
                RoomId = roomId.Value,
                StartDate = startDate,
                Capacity = 100,
                Visitors = 0
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(command));
            Assert.False(_unitOfWork.Committed);
        }

        [Fact]
        public async Task ExecuteAsync_WithCommandCapacityIgnored_SchedulesEventWithRoomCapacity()
        {
            // Arrange
            var movieId = new MovieId();
            var roomId = new RoomId();
            var startDate = DateTime.UtcNow.AddDays(1);

            var movie = Movie.Create(
                "Test Movie",
                "Description",
                DateTime.UtcNow.Year,
                120,
                "Action",
                "Actor 1, Actor 2",
                "12",
                "poster.jpg",
                movieId
            );

            var room = new Room(roomId, "Room 1", 100);

            await _movieRepository.Save(movie);
            await _roomRepository.AddAsync(room);

            var command = new ScheduleMovieEventCommand
            {
                MovieId = movieId.Value,
                RoomId = roomId.Value,
                StartDate = startDate,
                Capacity = 0,
                Visitors = 0
            };

            // Act
            await _useCase.ExecuteAsync(command);
            
            // Assert
            Assert.True(_unitOfWork.Committed);
            var events = await _movieEventRepository.GetEventsForMovieInRangeAsync(movieId, startDate.AddDays(-1), startDate.AddDays(1));
            var movieEvent = Assert.Single(events);
            Assert.Equal(100, movieEvent.Capacity);
        }
    }
}
