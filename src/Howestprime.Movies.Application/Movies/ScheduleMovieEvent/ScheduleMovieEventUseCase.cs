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

            // Extract time component for validation
            TimeSpan timeOfDay = command.StartDate.TimeOfDay;
            if (timeOfDay != new TimeSpan(15, 0, 0) && timeOfDay != new TimeSpan(19, 0, 0))
                throw new Exception("Time must be 15:00 or 19:00");

            // Make sure the date is in UTC
            DateTime eventTimeUtc = DateTime.SpecifyKind(command.StartDate, DateTimeKind.Utc);

            if (eventTimeUtc < DateTime.UtcNow)
                throw new Exception("Date must be in the future");

            if (command.Capacity <= 0)
                throw new Exception("Capacity must be greater than 0");

            var movieEvent = new MovieEvent
            {
                Id = Guid.NewGuid(),
                MovieId = command.MovieId,
                RoomId = command.RoomId,
                Time = eventTimeUtc,
                Capacity = command.Capacity,
                Visitors = command.Visitors
            };
            
            await _movieEventRepository.AddAsync(movieEvent);
            return new ScheduleMovieEventResult { EventId = movieEvent.Id };
        }
    }
}
