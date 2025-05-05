using System.Threading.Tasks;

namespace Howestprime.Movies.Application.Contracts.Ports
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}