using Howestprime.Movies.Domain.Shared;
using System.Threading.Tasks;
using Howestprime.Movies.Domain.Entities;

namespace Howestprime.Movies.Application.Contracts.Ports
{
    public interface IMovieRepository
    {
        Task<Howestprime.Movies.Domain.Entities.Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Howestprime.Movies.Domain.Entities.Movie> AddAsync(Howestprime.Movies.Domain.Entities.Movie movie, CancellationToken cancellationToken = default);
    }
}