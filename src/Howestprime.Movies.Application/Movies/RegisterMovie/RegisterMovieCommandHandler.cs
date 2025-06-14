using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Domain.Events;
using Domaincrafters.Domain;

namespace Howestprime.Movies.Application.Movies.RegisterMovie;

public class RegisterMovieCommandHandler
{
    private readonly IMovieRepository _movieRepository;

    public RegisterMovieCommandHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public virtual async Task<Movie> Handle(RegisterMovieCommand command)
    {
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


        DomainEventPublisher.Instance.Publish(
            Howestprime.Movies.Domain.Events.MovieRegistered.Create(
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