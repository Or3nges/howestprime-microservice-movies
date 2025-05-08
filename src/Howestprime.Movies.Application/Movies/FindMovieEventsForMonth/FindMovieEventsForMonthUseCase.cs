using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Application.Movies.FindMovieEventsForMonth
{
    public class FindMovieEventsForMonthUseCase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMovieEventRepository _movieEventRepository;
        private readonly IRoomRepository _roomRepository;

        public FindMovieEventsForMonthUseCase(
            IMovieRepository movieRepository,
            IMovieEventRepository movieEventRepository,
            IRoomRepository roomRepository)
        {
            _movieRepository = movieRepository;
            _movieEventRepository = movieEventRepository;
            _roomRepository = roomRepository;
        }

        public async Task<ScheduledMovieEventsResponse> ExecuteAsync(FindMovieEventsForMonthQuery query)
        {
            if (query.Month < 1 || query.Month > 12 || query.Year < 1)
                return new ScheduledMovieEventsResponse { Data = new List<ExtendedMovieEventData>() };

            var start = new DateTime(query.Year, query.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = start.AddMonths(1).AddTicks(-1);
            var events = await _movieEventRepository.GetEventsInRangeAsync(start, end);
            var result = new List<ExtendedMovieEventData>();
            
            foreach (var ev in events)
            {
                var movie = await _movieRepository.GetByIdAsync(ev.MovieId);
                var room = await _roomRepository.GetByIdAsync(ev.RoomId);
                
                if (movie == null || room == null) continue;
                
                result.Add(new ExtendedMovieEventData
                {
                    Id = ev.Id,
                    Time = DateTime.SpecifyKind(
                        new DateTime(ev.Date.Year, ev.Date.Month, ev.Date.Day)
                            .Add(ev.Time), 
                        DateTimeKind.Utc),
                    Room = new RoomData 
                    { 
                        Id = room.Id, 
                        Name = room.Name, 
                        Capacity = room.Capacity 
                    },
                    Movie = new MovieData
                    {
                        Id = movie.Id,
                        Title = movie.Title,
                        Genres = movie.Genre,
                        Actors = movie.Actors,
                        AgeRating = movie.AgeRating.ToString(),
                        Duration = movie.Duration,
                        PosterUrl = movie.PosterUrl
                    },
                    Capacity = ev.Capacity
                });
            }
            
            return new ScheduledMovieEventsResponse { Data = result };
        }
    }

    public class ScheduledMovieEventsResponse
    {
        public List<ExtendedMovieEventData> Data { get; set; } = new List<ExtendedMovieEventData>();
    }
}
