using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Domain.MovieEvent;
using Howestprime.Movies.Domain.Room;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;

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
            return await HandleAsync(query);
        }

        public async Task<MovieData> HandleAsync(FindMovieByIdWithEventsQuery query)
        {
            var movie = await _movieRepository.GetByIdAsync(query.MovieId);
            if (movie == null)
                throw new InvalidOperationException("Movie not found.");

            var events = await _movieEventRepository.GetEventsForMovieInRangeAsync(
                query.MovieId,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMonths(1));

            var movieData = new MovieData
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
                Events = new List<MovieEventData>()
            };

            foreach (var evt in events)
            {
                var room = await _roomRepository.GetByIdAsync(evt.RoomId);
                if (room == null)
                    continue;

                movieData.Events.Add(new MovieEventData
                {
                    Id = evt.Id.Value,
                    Time = evt.Time,
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

            return movieData;
        }
    }
}
