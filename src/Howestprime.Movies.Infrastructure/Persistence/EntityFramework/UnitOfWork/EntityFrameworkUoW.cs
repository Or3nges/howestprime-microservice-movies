using Domaincrafters.Application;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.UnitOfWork;

public class EntityFrameworkUoW(
    DomainContextBase domainContext,
    ILogger<EntityFrameworkUoW> logger
) : IUnitOfWork
{
    private readonly DbContext _domainContext = domainContext;
    private readonly ILogger<EntityFrameworkUoW> _logger = logger;

    public Task Do(Func<Task>? action = null)
    {
        if (action is not null)
            _logger.LogWarning("Delegates passed to EF unit of work are ignored.");

        return _domainContext.SaveChangesAsync();
    }
}