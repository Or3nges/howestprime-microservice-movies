using System;
using System.Threading.Tasks;
using Howestprime.Movies.Domain.Entities;
using System.Collections.Generic;

namespace Howestprime.Movies.Application.Contracts.Ports
{
    public interface IMovieEventRepository
    {
        Task<MovieEvent?> GetByRoomDateTimeAsync(Guid roomId, DateTime date, TimeSpan time);
        Task AddAsync(MovieEvent movieEvent);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<MovieEvent>> GetEventsForMovieInRangeAsync(Guid movieId, DateTime start, DateTime end);
        Task<IEnumerable<MovieEvent>> GetEventsInRangeAsync(DateTime start, DateTime end);
    }
}
