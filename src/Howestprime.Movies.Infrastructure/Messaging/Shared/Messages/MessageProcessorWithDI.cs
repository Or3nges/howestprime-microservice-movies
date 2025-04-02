namespace Howestprime.Movies.Infrastructure.Messaging.Shared;

using System;
using System.Threading.Tasks;
using Howestprime.Movies.Infrastructure.Messaging.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class MessageProcessorWithDI(
    IServiceProvider serviceProvider,
    ILogger<MessageProcessorWithDI> logger
) : IAmqpMessageProcessor
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<MessageProcessorWithDI> _logger = logger;

    public Task ProcessMessage(ConsumerContext ctx)
    {
        _logger.LogInformation(
            "Processing message from exchange {ExchangeName} with event {EventName} and message: {Message}",
             ctx.ExchangeName, ctx.EventName, ctx.Message);

        IServiceProvider scopedProvider = _serviceProvider.CreateScope().ServiceProvider;

        return InvokeController(ctx, scopedProvider);
    }

    private Task InvokeController(
        ConsumerContext ctx,
        IServiceProvider scopedProvider)
    {
        IControllerFactory controllerFactory = scopedProvider.GetService<IControllerFactory>()!;
        IController<ConsumerContext> controller = controllerFactory.CreateController(ctx);

        return controller.Handle(ctx);
    }

}