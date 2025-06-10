using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly MoviesDbContext _context;
        public RoomRepository(MoviesDbContext context)
        {
            _context = context;
        }

        public async Task<Room?> GetByIdAsync(Guid id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _context.Rooms.ToListAsync();
        }

        public async Task<Room> AddAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task SeedRoomsAsync()
        {
            if (!await _context.Rooms.AnyAsync())
            {
                _context.Rooms.Add(new Room { Id = Guid.NewGuid(), Name = "Blue Room", Capacity = 100 });
                _context.Rooms.Add(new Room { Id = Guid.NewGuid(), Name = "Red Room", Capacity = 80 });
                await _context.SaveChangesAsync();
            }
        }
    }
}
