using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Entities;
using Howestprime.Movies.Application.Movies.RegisterMovie;

namespace Howestprime.Movies.Application
{
    public class RegisterMovieUseCase
    {
        private readonly IMovieRepository _movieRepository;

        public RegisterMovieUseCase(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<Movie> ExecuteAsync(RegisterMovieCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Title))
                throw new ArgumentException("Title is required");
            if (command.Duration <= 0)
                throw new ArgumentException("Duration must be positive");

            var movie = new Movie(
                Guid.NewGuid(),
                command.Title,
                command.Description ?? string.Empty,
                command.Genre ?? string.Empty,
                command.Actors ?? string.Empty,
                command.AgeRating.ToString(),
                command.Duration,
                command.PosterUrl ?? string.Empty
            );

            return await _movieRepository.AddAsync(movie);
        }
    }
}