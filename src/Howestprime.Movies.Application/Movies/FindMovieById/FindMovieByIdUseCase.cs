using System;
using System.Threading.Tasks;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Shared;
using Howestprime.Movies.Domain.MovieEvent;
using System.Collections.Generic;
using Howestprime.Movies.Domain.Shared.Exceptions;

namespace Howestprime.Movies.Application.Movies.FindMovieById
{
    public class FindMovieByIdUseCase
    {
        private readonly IMovieRepository _movieRepository;
        public FindMovieByIdUseCase(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<MovieData> ExecuteAsync(MovieByIdQuery query)
        {
            var movie = await _movieRepository.GetByIdAsync(query.Id);
            if (movie == null)
                throw new NotFoundException($"Movie with id {query.Id} not found");
            return new MovieData
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
        }
    }
}
