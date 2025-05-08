using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Application.Movies.FindMoviesWithEventsInTimeframe
{
    public class FindMoviesWithEventsInTimeframeUseCase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMovieEventRepository _movieEventRepository;
        private readonly IRoomRepository _roomRepository;

        public FindMoviesWithEventsInTimeframeUseCase(
            IMovieRepository movieRepository,
            IMovieEventRepository movieEventRepository,
            IRoomRepository roomRepository)
        {
            _movieRepository = movieRepository;
            _movieEventRepository = movieEventRepository;
            _roomRepository = roomRepository;
        }

        public async Task<MoviesWithEventsResponse> ExecuteAsync(FindMoviesWithEventsInTimeframeQuery query)
        {
            if (query == null)
                return new MoviesWithEventsResponse { Data = new List<MovieData>() };

            var today = DateTime.UtcNow.Date;
            var end = today.AddDays(14);
            var movies = await _movieRepository.FindByFiltersAsync(query.Title, query.Genre);
            var result = new List<MovieData>();
            foreach (var movie in movies)
            {
                var events = await _movieEventRepository.GetEventsForMovieInRangeAsync(movie.Id, today, end);
                if (events == null || !events.Any()) continue;
                var eventDatas = new List<MovieEventData>();
                foreach (var ev in events)
                {
                    var room = await _roomRepository.GetByIdAsync(ev.RoomId);
                    if (room == null) continue;
                    
                    eventDatas.Add(new MovieEventData
                    {
                        Id = ev.Id,
                        Date = DateOnly.FromDateTime(ev.Date),
                        Time = TimeOnly.FromTimeSpan(ev.Time),
                        Room = new RoomData { Id = room.Id, Name = room.Name, Capacity = room.Capacity },
                        Capacity = ev.Capacity,
                        Visitors = 0
                    });
                }
                result.Add(new MovieData
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Genres = movie.Genre,
                    Actors = movie.Actors,
                    AgeRating = movie.AgeRating,
                    Duration = movie.Duration,
                    PosterUrl = movie.PosterUrl,
                    Events = eventDatas
                });
            }
            return new MoviesWithEventsResponse { Data = result };
        }
    }

    public class MoviesWithEventsResponse
    {
        public List<MovieData> Data { get; set; } = new List<MovieData>();
    }
}
