using System;
using System.Threading.Tasks;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Domain.MovieEvent;
using Howestprime.Movies.Domain.Room;
using Howestprime.Movies.Application.Contracts.Ports;

namespace Howestprime.Movies.Application.Movies.ScheduleMovieEvent
{
    public class ScheduleMovieEventUseCase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMovieEventRepository _movieEventRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleMovieEventUseCase(
            IMovieRepository movieRepository,
            IRoomRepository roomRepository,
            IMovieEventRepository movieEventRepository,
            IUnitOfWork unitOfWork)
        {
            _movieRepository = movieRepository;
            _roomRepository = roomRepository;
            _movieEventRepository = movieEventRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(ScheduleMovieEventCommand command)
        {
            var movieId = new MovieId(command.MovieId);
            var roomId = new RoomId(command.RoomId);

            var movie = await _movieRepository.GetByIdAsync(movieId);
            if (movie == null)
                throw new Exception($"Movie with ID {movieId} not found.");

            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
                throw new Exception($"Room with ID {roomId} not found.");

            var existingEvent = await _movieEventRepository.GetByRoomDateTimeAsync(
                roomId,
                command.StartDate.Date,
                command.StartDate.TimeOfDay);

            if (existingEvent != null)
                throw new InvalidOperationException("A movie event already exists in this room at the specified time.");

            var movieEvent = new MovieEvent(
                new MovieEventId(),
                movieId,
                roomId,
                command.StartDate,
                room.Capacity
            );

            await _movieEventRepository.AddAsync(movieEvent);
            await _unitOfWork.CommitAsync();
        }
    }
}
