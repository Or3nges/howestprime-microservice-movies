using System;
using System.Linq;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Movies.ScheduleMovieEvent;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Domain.Room;
using UnitTests.Shared;
using Xunit;

namespace UnitTests.Application
{
    public class ScheduleMovieEventCommandHandlerTests
    {
        private readonly FakeMovieRepository _movieRepository;
        private readonly FakeRoomRepository _roomRepository;
        private readonly FakeMovieEventRepository _movieEventRepository;
        private readonly ScheduleMovieEventCommandHandler _handler;

        public ScheduleMovieEventCommandHandlerTests()
        {
            _movieRepository = new FakeMovieRepository();
            _roomRepository = new FakeRoomRepository();
            _movieEventRepository = new FakeMovieEventRepository();
            _handler = new ScheduleMovieEventCommandHandler(_movieRepository, _roomRepository, _movieEventRepository);
        }

        [Fact]
        public async Task Handle_WithValidCommand_CreatesAndSavesMovieEvent()
        {
            // Arrange
            var movie = Movie.Create("Test Movie", "Desc", 2024, 120, "Genre", "Actors", "12", "poster.jpg", new MovieId());
            var room = new Room(new RoomId(), "Room 1", 100);
            await _movieRepository.Save(movie);
            await _roomRepository.AddAsync(room);

            var command = new ScheduleMovieEventCommand
            {
                MovieId = movie.Id.Value,
                RoomId = room.Id.Value,
                StartDate = DateTime.UtcNow.AddDays(1),
                Capacity = 100
            };

            // Act
            await _handler.Handle(command);

            // Assert
            var movieEvent = _movieEventRepository.Events.FirstOrDefault();
            Assert.NotNull(movieEvent);
            Assert.Equal(movie.Id, movieEvent.MovieId);
            Assert.Equal(room.Id, movieEvent.RoomId);
        }
    }
} 