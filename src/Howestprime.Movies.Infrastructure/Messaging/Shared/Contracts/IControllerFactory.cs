namespace Howestprime.Movies.Infrastructure.Messaging.Shared.Contracts;

public interface IControllerFactory
{
    IController<ConsumerContext> CreateController(ConsumerContext consumerContext);
}
