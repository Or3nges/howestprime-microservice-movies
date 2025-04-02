using Domaincrafters.Application;

namespace UnitTests.Shared;

public sealed class MockUnitOfWork : IUnitOfWork
{
    public async Task Do(Func<Task>? action = null)
    {
        if (action is not null)
            await action();
    }
}