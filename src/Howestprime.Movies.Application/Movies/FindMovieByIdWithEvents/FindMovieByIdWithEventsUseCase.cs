using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Application.Movies.FindMovieByIdWithEvents
{
    public class FindMovieByIdWithEventsUseCase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMovieEventRepository _movieEventRepository;
        private readonly IRoomRepository _roomRepository;

        public FindMovieByIdWithEventsUseCase(
            IMovieRepository movieRepository,
            IMovieEventRepository movieEventRepository,
            IRoomRepository roomRepository)
        {
            _movieRepository = movieRepository;
            _movieEventRepository = movieEventRepository;
            _roomRepository = roomRepository;
        }

        public async Task<MovieData> ExecuteAsync(FindMovieByIdWithEventsQuery query)
        {
            var movie = await _movieRepository.ById(query.MovieId);
            if (movie == null)
                throw new Exception("Movie not found");

            var today = DateTime.UtcNow.Date;
            var end = today.AddDays(14);
            var events = await _movieEventRepository.GetEventsForMovieInRangeAsync(query.MovieId, today, end);
            if (events == null || !events.Any())
                throw new Exception("No events found for this movie in the next 14 days");

            var eventDatas = new List<MovieEventData>();
            foreach (var ev in events)
            {
                var room = await _roomRepository.ById(ev.RoomId);
                if (room == null) continue;
                
                DateTime eventDateTime = DateTime.SpecifyKind(ev.Time, DateTimeKind.Utc);
                
                eventDatas.Add(new MovieEventData
                {
                    Id = ev.Id,
                    DateTime = eventDateTime,
                    Room = new RoomData { Id = room.Id, Name = room.Name, Capacity = room.Capacity },
                    Capacity = ev.Capacity,
                    Visitors = ev.Visitors
                });
            }

            return new MovieData
            {
                Id = movie.Id,
                Title = movie.Title,
                Genres = movie.Genre,
                Actors = movie.Actors,
                AgeRating = movie.AgeRating,
                Duration = movie.Duration,
                PosterUrl = movie.PosterUrl,
                Events = eventDatas
            };
        }
    }
}
