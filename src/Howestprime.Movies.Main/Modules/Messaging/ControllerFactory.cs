using Howestprime.Movies.Infrastructure.Messaging.Shared.Contracts;

namespace Howestprime.Movies.Main.Modules.Messaging;

public class ControllerFactory(
    IServiceProvider serviceProvider
) : IControllerFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IController<ConsumerContext> CreateController(ConsumerContext consumerContext)
    {
        return consumerContext.OperationId switch
        {
            _ => throw new InvalidOperationException($"Operation with id {consumerContext.OperationId} not found")
        };
    }
}