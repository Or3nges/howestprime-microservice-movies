using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Repositories
{
    public class MovieEventRepository : IMovieEventRepository
    {
        private readonly MoviesDbContext _context;

        public MovieEventRepository(MoviesDbContext context)
        {
            _context = context;
        }

        public async Task<MovieEvent?> GetByRoomDateTimeAsync(Guid roomId, DateTime date, TimeSpan time)
        {
            // Combine date and time for comparison with the new Time field
            var eventTime = date.Add(time);
            
            return await _context.MovieEvents
                .Where(e => e.RoomId == roomId)
                .Where(e => e.Time == eventTime)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MovieEvent>> GetEventsForMovieInRangeAsync(Guid movieId, DateTime start, DateTime end)
        {
            return await _context.MovieEvents
                .Where(e => e.MovieId == movieId)
                .Where(e => e.Time >= start && e.Time <= end)
                .ToListAsync();
        }

        public async Task<IEnumerable<MovieEvent>> GetEventsInRangeAsync(DateTime start, DateTime end)
        {
            return await _context.MovieEvents
                .Where(e => e.Time >= start && e.Time <= end)
                .ToListAsync();
        }

        public async Task<MovieEvent> GetByIdWithBookingsAsync(Guid movieEventId)
        {
            var result = await _context.MovieEvents
                .Include(e => e.Bookings)
                .FirstOrDefaultAsync(e => e.Id == movieEventId);
            
            if (result == null)
                throw new InvalidOperationException($"MovieEvent with ID {movieEventId} not found");
                
            return result;
        }

        public async Task AddAsync(MovieEvent movieEvent)
        {
            _context.MovieEvents.Add(movieEvent);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MovieEvent movieEvent)
        {
            _context.MovieEvents.Update(movieEvent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var movieEvent = await _context.MovieEvents.FindAsync(id);
            if (movieEvent != null)
            {
                _context.MovieEvents.Remove(movieEvent);
                await _context.SaveChangesAsync();
            }
        }
    }
}
