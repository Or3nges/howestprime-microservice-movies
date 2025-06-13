using System;
using System.Threading.Tasks;
using Howestprime.Movies.Domain.Entities;
using System.Collections.Generic;

namespace Howestprime.Movies.Application.Contracts.Ports
{
    public interface IMovieEventRepository
    {
        Task<MovieEvent?> GetByRoomDateTimeAsync(RoomId roomId, DateTime date, TimeSpan time);
        Task AddAsync(MovieEvent movieEvent);
        Task DeleteAsync(MovieEventId id);
        Task<IEnumerable<MovieEvent>> GetEventsForMovieInRangeAsync(MovieId movieId, DateTime start, DateTime end);
        Task<IEnumerable<MovieEvent>> GetEventsInRangeAsync(DateTime start, DateTime end);
        Task<MovieEvent> GetByIdWithBookingsAsync(MovieEventId movieEventId);
        Task UpdateAsync(MovieEvent movieEvent);
    }
}
