using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Entities;
using Howestprime.Movies.Application.Movies.RegisterMovie;
using Domaincrafters.Domain;
using Howestprime.Movies.Domain.Events;

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
                new MovieId(),
                command.Title ?? "Untitled",
                command.Description ?? string.Empty,
                command.Year,
                command.Genre ?? string.Empty,
                command.Actors ?? string.Empty,
                command.AgeRating.ToString(),
                command.Duration,
                command.PosterUrl ?? string.Empty
            );

            await _movieRepository.Save(movie);

            // Notify other services via domain event
            DomainEventPublisher.Instance.Publish(
                MovieRegistered.Create(
                    movie.Id.Value,
                    movie.Title,
                    movie.Year,
                    movie.Duration,
                    movie.Genre,
                    movie.Actors,
                    int.TryParse(movie.AgeRating, out var rating) ? rating : 0,
                    movie.PosterUrl
                )
            );

            return movie;
        }
    }
}