using System;
using System.Threading.Tasks;
using Howestprime.Movies.Domain.Entities;

namespace Howestprime.Movies.Application.Contracts.Ports
{
    public interface IRoomRepository
    {
        Task<Room?> GetByIdAsync(Guid id);
        Task SeedRoomsAsync();
    }
}
