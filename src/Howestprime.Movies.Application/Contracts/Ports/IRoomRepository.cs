using System;
using System.Threading.Tasks;
using Howestprime.Movies.Domain.Entities;
using Domaincrafters.Domain;

namespace Howestprime.Movies.Application.Contracts.Ports
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
