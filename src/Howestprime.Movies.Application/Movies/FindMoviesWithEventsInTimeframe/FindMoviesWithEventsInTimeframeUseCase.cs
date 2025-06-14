using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Domain.MovieEvent;
using Howestprime.Movies.Domain.Room;

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

            var startDate = query.StartDate ?? DateTime.UtcNow;
            var endDate = query.EndDate ?? startDate.AddDays(14);


            startDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            endDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);

            var events = await _movieEventRepository.GetEventsInRangeAsync(startDate, endDate);
            var movieIds = events.Select(e => e.MovieId).Distinct().ToList();
            var movies = new List<MovieData>();

            foreach (var movieId in movieIds)
            {
                var movie = await _movieRepository.GetByIdAsync(movieId);
                if (movie == null) continue;

                var movieEvents = events.Where(e => e.MovieId.Equals(movieId)).ToList();
                var movieEventDatas = new List<MovieEventData>();

                foreach (var evt in movieEvents)
                {
                    var room = await _roomRepository.GetByIdAsync(evt.RoomId);
                    if (room == null) continue;


                    var eventTime = DateTime.SpecifyKind(evt.Time, DateTimeKind.Utc);

                    movieEventDatas.Add(new MovieEventData
                    {
                        Id = evt.Id.Value,
                        Time = eventTime,
                        MovieId = evt.MovieId.Value,
                        Capacity = evt.Capacity,
                        Room = new RoomData
                        {
                            Id = room.Id.Value,
                            Name = room.Name,
                            Capacity = room.Capacity
                        }
                    });
                }

                movies.Add(new MovieData
                {
                    Id = movie.Id.Value,
                    Title = movie.Title,
                    Description = movie.Description,
                    Year = movie.Year,
                    Genre = movie.Genre,
                    Actors = movie.Actors,
                    AgeRating = int.Parse(movie.AgeRating),
                    Duration = movie.Duration,
                    PosterUrl = movie.PosterUrl,
                    Events = movieEventDatas
                });
            }

            return new MoviesWithEventsResponse { Data = movies };
        }
    }

    public class MoviesWithEventsResponse
    {
        public List<MovieData> Data { get; set; } = new List<MovieData>();
    }
}
