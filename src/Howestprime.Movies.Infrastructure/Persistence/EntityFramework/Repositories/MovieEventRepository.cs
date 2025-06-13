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

        public async Task<MovieEvent?> GetByRoomDateTimeAsync(RoomId roomId, DateTime date, TimeSpan time)
        {
            return await _context.MovieEvents
                .FirstOrDefaultAsync(e => e.RoomId == roomId && e.Time.Date == date.Date && e.Time.TimeOfDay == time);
        }

        public async Task<IEnumerable<MovieEvent>> GetEventsForMovieInRangeAsync(MovieId movieId, DateTime start, DateTime end)
        {
            return await _context.MovieEvents
                .Where(e => e.MovieId == movieId && e.Time >= start && e.Time <= end)
                .ToListAsync();
        }

        public async Task<IEnumerable<MovieEvent>> GetEventsInRangeAsync(DateTime start, DateTime end)
        {
            return await _context.MovieEvents
                .Where(e => e.Time >= start && e.Time <= end)
                .ToListAsync();
        }

        public async Task<MovieEvent> GetByIdWithBookingsAsync(MovieEventId movieEventId)
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
            await _context.MovieEvents.AddAsync(movieEvent);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MovieEvent movieEvent)
        {
            _context.MovieEvents.Update(movieEvent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(MovieEventId id)
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
