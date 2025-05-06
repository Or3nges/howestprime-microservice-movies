using System;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Entities;

namespace Howestprime.Movies.Application.Movies.ScheduleMovieEvent
{
    public class ScheduleMovieEventUseCase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMovieEventRepository _movieEventRepository;
        private readonly IRoomRepository _roomRepository;

        public ScheduleMovieEventUseCase(
            IMovieRepository movieRepository,
            IMovieEventRepository movieEventRepository,
            IRoomRepository roomRepository)
        {
            _movieRepository = movieRepository;
            _movieEventRepository = movieEventRepository;
            _roomRepository = roomRepository;
        }

        public async Task<ScheduleMovieEventResult> ExecuteAsync(ScheduleMovieEventCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var movie = await _movieRepository.GetByIdAsync(command.MovieId);
            if (movie == null)
                throw new Exception("Movie not found");

            var room = await _roomRepository.GetByIdAsync(command.RoomId);
            if (room == null)
                throw new Exception("Room not found");

            if (command.Time != new TimeSpan(15, 0, 0) && command.Time != new TimeSpan(19, 0, 0))
                throw new Exception("Time must be 15:00 or 19:00");

            if (command.Date.Date < DateTime.UtcNow.Date)
                throw new Exception("Date must be in the future");

            if (command.Capacity <= 0)
                throw new Exception("Capacity must be greater than 0");

            var existing = await _movieEventRepository.GetByRoomDateTimeAsync(command.RoomId, command.Date, command.Time);
            if (existing != null)
                await _movieEventRepository.DeleteAsync(existing.Id);

            var movieEvent = new MovieEvent
            {
                Id = Guid.NewGuid(),
                MovieId = command.MovieId,
                RoomId = command.RoomId,
                Date = command.Date.Date,
                Time = command.Time,
                Capacity = command.Capacity
            };
            await _movieEventRepository.AddAsync(movieEvent);
            return new ScheduleMovieEventResult { EventId = movieEvent.Id };
        }
    }
}
