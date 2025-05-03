using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Entities;

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