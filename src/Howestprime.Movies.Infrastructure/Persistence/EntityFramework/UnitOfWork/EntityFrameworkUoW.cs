using Domaincrafters.Application;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.UnitOfWork;

public class EntityFrameworkUoW : Howestprime.Movies.Application.Contracts.Ports.IUnitOfWork
{
    private readonly DbContext _domainContext;
    private readonly ILogger<EntityFrameworkUoW> _logger;

    public EntityFrameworkUoW(DbContext domainContext, ILogger<EntityFrameworkUoW> logger)
    {
        _domainContext = domainContext;
        _logger = logger;
    }

    public Task Do(Func<Task>? action = null)
    {
        if (action is not null)
            _logger.LogWarning("Delegates passed to EF unit of work are ignored.");

        return _domainContext.SaveChangesAsync();
    }

    public Task CommitAsync()
    {
        return _domainContext.SaveChangesAsync();
    }
}