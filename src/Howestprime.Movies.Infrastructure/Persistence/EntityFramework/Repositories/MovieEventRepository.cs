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
            return await _context.MovieEvents.FirstOrDefaultAsync(e => e.RoomId == roomId && e.Date == date && e.Time == time);
        }

        public async Task<IEnumerable<MovieEvent>> GetEventsForMovieInRangeAsync(Guid movieId, DateTime start, DateTime end)
        {
            return await _context.MovieEvents
                .Where(e => e.MovieId == movieId && e.Date >= start && e.Date <= end)
                .ToListAsync();
        }

        public async Task<IEnumerable<MovieEvent>> GetEventsInRangeAsync(DateTime start, DateTime end)
        {
            return await _context.MovieEvents
                .Where(e => e.Date >= start && e.Date <= end)
                .ToListAsync();
        }

        public async Task AddAsync(MovieEvent movieEvent)
        {
            _context.MovieEvents.Add(movieEvent);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.MovieEvents.FindAsync(id);
            if (entity != null)
            {
                _context.MovieEvents.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        
                public async Task<MovieEvent> GetByIdWithBookingsAsync(Guid movieEventId)
        {
            return await _context.MovieEvents
                .Include(e => e.Bookings)
                .FirstOrDefaultAsync(e => e.Id == movieEventId);
        }
        public async Task UpdateAsync(MovieEvent movieEvent)
        {
            var lastBooking = movieEvent.Bookings.LastOrDefault();
            if (lastBooking == null)
            {
                return;
            }
            
            int visitorsToAdd = lastBooking.StandardVisitors + lastBooking.DiscountVisitors;
            
            await _context.Database.ExecuteSqlRawAsync(
                "UPDATE \"MovieEvents\" SET \"Visitors\" = \"Visitors\" + {0} WHERE \"Id\" = {1}",
                visitorsToAdd,
                movieEvent.Id);
            
            _context.Set<Booking>().Add(lastBooking);
            
            await _context.SaveChangesAsync();
        }
    }
}
