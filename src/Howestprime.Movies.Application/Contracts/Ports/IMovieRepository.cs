using Howestprime.Movies.Domain.Shared;
using System.Threading.Tasks;
using Howestprime.Movies.Domain.Entities;
using Domaincrafters.Domain;

namespace Howestprime.Movies.Application.Contracts.Ports
{
    public interface IMovieRepository : IRepository<Movie, MovieId>
    {
    }
}