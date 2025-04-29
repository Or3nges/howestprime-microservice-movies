using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Application.Contracts.Ports
{
    public interface IMovieRepository
    {
        Task<Movie> AddAsync(Movie movie);
    }
}