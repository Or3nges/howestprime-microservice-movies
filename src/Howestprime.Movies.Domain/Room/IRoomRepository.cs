using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Domaincrafters.Domain;
using Howestprime.Movies.Domain.Room;

namespace Howestprime.Movies.Domain.Room
{
    public interface IRoomRepository
    {
        Task<Room?> GetByIdAsync(RoomId id);
        Task<Room?> ById(RoomId id);
        Task<Room> AddAsync(Room room);
        Task<IEnumerable<Room>> GetAllAsync();
        Task SeedRoomsAsync();
    }
}
