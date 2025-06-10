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
            command.Title ?? "Untitled",
            command.Description ?? "",
            command.Genre ?? "",
            command.Actors ?? "",
            command.AgeRating ?? "",
            command.Duration,
            command.PosterUrl ?? ""
        );
        
        await _movieRepository.AddAsync(movie);
        return movie;
    }
}