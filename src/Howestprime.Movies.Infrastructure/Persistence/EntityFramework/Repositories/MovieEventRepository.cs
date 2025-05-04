using System;
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
    }
}
