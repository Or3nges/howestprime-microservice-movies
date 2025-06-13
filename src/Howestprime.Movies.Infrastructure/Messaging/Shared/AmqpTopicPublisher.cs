using Domaincrafters.Domain;
using Howestprime.Movies.Infrastructure.Messaging.Shared.Contracts;
using Howestprime.Movies.Infrastructure.Messaging.Shared.Messages;

namespace Howestprime.Movies.Infrastructure.Messaging.Shared;

public sealed class AmqpTopicPublisher(
    IAmqpBroker amqpBroker,
    string exchangeName,
    IList<string> allowedTopics,
    string? contentType = "application/json"
) : IDomainEventSubscriber
{
    private readonly IAmqpBroker _amqpBroker = amqpBroker;
    private readonly string _exchangeName = exchangeName;
    private readonly IList<string> _allowedTopics = allowedTopics;
    private readonly string? _contentType = contentType;

    public void HandleEvent(IDomainEvent domainEvent)
    {
        string routingKey = RoutingKey(domainEvent);

        if (IsSubscribedTo(domainEvent))
        {
            // Log so we can trace event handling.
            Console.WriteLine($"[AmqpTopicPublisher] Publishing {domainEvent.QualifiedEventName} to {_exchangeName}:{routingKey}");

            _amqpBroker.PublishOnTopic(
                _exchangeName,
                routingKey,
                 AmqpMessageConverter.Serialize(domainEvent, _contentType)
            );
        }
    }
    public bool IsSubscribedTo(IDomainEvent domainEvent)
    {
        return _allowedTopics.Contains(RoutingKey(domainEvent));
    }

    // Builds the routing-key according to the AsyncAPI specification.
    // Example: exchangeName = "howestprime" & event = "MovieRegistered"  →
    //           "howestprime.movies.MovieRegistered"
    private string RoutingKey(IDomainEvent domainEvent)
    {
        return $"{_exchangeName}.movies.{domainEvent.QualifiedEventName}";
    }
}
