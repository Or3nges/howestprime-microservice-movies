using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Events;
using Microsoft.Extensions.Logging;
using Howestprime.Movies.Infrastructure.Messaging.Shared;
using Howestprime.Movies.Infrastructure.Messaging.Shared.Contracts;

namespace Howestprime.Movies.Infrastructure.Messaging.Publishers;

public class MovieEventPublisher : IEventPublisher
{
    private readonly ILogger<MovieEventPublisher> _logger;
    private readonly IAmqpBroker _amqpBroker;

    public MovieEventPublisher(ILogger<MovieEventPublisher> logger, IAmqpBroker amqpBroker)
    {
        _logger = logger;
        _amqpBroker = amqpBroker;
    }

    public async Task PublishAsync(BookingOpened bookingOpenedEvent)
    {

        var message = System.Text.Json.JsonSerializer.Serialize(bookingOpenedEvent);

        string exchange = "howestprime";

        string routingKey = "howestprime.movies.BookingOpened";
        _logger.LogInformation("Publishing BookingOpened event to AMQP: {Message}", message);
        await _amqpBroker.PublishOnTopic(exchange, routingKey, message);
    }
}
