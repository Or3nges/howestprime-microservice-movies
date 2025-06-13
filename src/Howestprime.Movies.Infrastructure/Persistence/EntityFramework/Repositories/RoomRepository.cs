using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Domaincrafters.Domain;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly MoviesDbContext _context;

        public RoomRepository(MoviesDbContext context)
        {
            _context = context;
        }

        public async Task<Room?> GetByIdAsync(RoomId id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task<Room?> ById(RoomId id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task<Room> AddAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _context.Rooms.ToListAsync();
        }

        public async Task SeedRoomsAsync()
        {
            if (!await _context.Rooms.AnyAsync())
            {
                var rooms = new List<Room>
                {
                    new Room(new RoomId(), "Room 1", 100),
                    new Room(new RoomId(), "Room 2", 150),
                    new Room(new RoomId(), "Room 3", 200)
                };

                await _context.Rooms.AddRangeAsync(rooms);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Save(Room room)
        {
            if (await _context.Rooms.FindAsync(room.Id) == null)
            {
                await _context.Rooms.AddAsync(room);
            }
            else
            {
                _context.Rooms.Update(room);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Room room)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }
    }
}
