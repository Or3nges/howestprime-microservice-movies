using System;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Shared;
using Howestprime.Movies.Domain.Entities;

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
                throw new Exception($"Movie with id {query.Id} not found");
            return new MovieData
            {
                Id = movie.Id,
                Title = movie.Title,
                Genres = movie.Genre,
                Actors = movie.Actors,
                AgeRating = movie.AgeRating,
                Duration = movie.Duration,
                PosterUrl = movie.PosterUrl
            };
        }
    }
}
