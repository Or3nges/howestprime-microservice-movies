using Howestprime.Movies.Infrastructure.Messaging.Shared.Contracts;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Howestprime.Movies.Infrastructure.Messaging.Shared;

public class DefaultAmqpBroker(
    ConnectionFactory factory,
    BrokerConfig brokerConfig,
    ILogger<IAmqpBroker> logger
) : IAmqpBroker
{
    private IChannel? _channel;
    private readonly ConnectionFactory _factory = factory;
    private readonly Exchange[] _exchanges = brokerConfig.Exchanges;
    private readonly IList<IAmqpMessageProcessor> _messageProcessors = [];
    private readonly ILogger<IAmqpBroker> _logger = logger;

    public async Task Connect()
    {
        IConnection conn = await _factory.CreateConnectionAsync();
        _channel = await conn.CreateChannelAsync();
        foreach (var exchange in _exchanges)
            await _channel.ExchangeDeclareAsync(
                exchange.Name, 
                exchange.Type, 
                durable: false, 
                autoDelete: true,
                passive: false
            );
        _logger.LogInformation("Connected to AMQP broker with host {Host}.", _factory.Uri);
    }

    public async Task ConsumeFromTopic(ConsumerConfig consumerConfig)
    {
        EnsureValidExchange(consumerConfig.ExchangeName);

        string queueName = (await _channel!.QueueDeclareAsync()).QueueName;

        await _channel!.QueueBindAsync(
            queueName,
            consumerConfig.ExchangeName,
            consumerConfig.Event
        );

        AsyncEventingBasicConsumer consumer = new(_channel);

        consumer.ReceivedAsync += (model, ea) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(ea.Body.ToArray());

            ConsumerContext ctx = CreateConsumerContext(consumerConfig, message);

            if (ea.BasicProperties.IsContentTypePresent())
                ctx.ContentType = ea.BasicProperties.ContentType;

            foreach (var processor in _messageProcessors)
                processor.ProcessMessage(ctx);

            return Task.CompletedTask;
        };

        await _channel.BasicConsumeAsync(queueName, autoAck: true, consumer);
    }

    public Task PublishOnTopic(string exchangeName, string routingKey, string message)
    {
        EnsureValidExchange(exchangeName);

        BasicProperties? props = new()
        {
            ContentType = "application/json"
        };
        
        _logger.LogInformation("Published message: {Body}, with routing key: {RoutingKey}", message, routingKey);

        return _channel!.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: props,
            body: System.Text.Encoding.UTF8.GetBytes(message)
        ).AsTask();
    }

    public IAmqpBroker AddMessageProcessor(IAmqpMessageProcessor messageProcessor)
    {
        _messageProcessors.Add(messageProcessor);
        return this;
    }

    private void EnsureValidExchange(string exchangeName)
    {
        if (_exchanges.All(exchange => exchange.Name != exchangeName))
            throw new ArgumentException($"Exchange {exchangeName} is not valid.");
    }

    private static ConsumerContext CreateConsumerContext(ConsumerConfig config, string message)
    {
        return new(config.ExchangeName, config.Event, config.OperationId, message, null);
    }
}