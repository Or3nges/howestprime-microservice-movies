using System;
using System.Threading.Tasks;
using Howestprime.Movies.Domain.Entities;

namespace Howestprime.Movies.Application.Contracts.Ports
{
    public interface IMovieEventRepository
    {
        Task<MovieEvent?> GetByRoomDateTimeAsync(Guid roomId, DateTime date, TimeSpan time);
        Task AddAsync(MovieEvent movieEvent);
        Task DeleteAsync(Guid id);
    }
}
