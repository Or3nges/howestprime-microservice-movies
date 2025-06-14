using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Howestprime.Movies.Domain.Movie
{
    public interface IMovieRepository
    {
        Task Save(Movie entity);
        Task<Movie?> GetByIdAsync(MovieId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Movie>> FindByFiltersAsync(string? title, string? genre, CancellationToken cancellationToken = default);
        Task<Movie?> ById(MovieId id);
        Task Remove(Movie movie);
        Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken = default);
    }
}