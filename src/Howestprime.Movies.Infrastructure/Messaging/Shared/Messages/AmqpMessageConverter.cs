using System.Text.Json;
using Domaincrafters.Domain;
using Howestprime.Movies.Infrastructure.Messaging.Shared.Contracts;

namespace Howestprime.Movies.Infrastructure.Messaging.Shared.Messages;

public static class AmqpMessageConverter
{
    public static Type ParseBody<Type>(ConsumerContext ctx)
    {
        return ctx.ContentType switch
        {
            "application/json" => ParseJson<Type>(ctx.Message),
            _ => ParseString<Type>(ctx.Message)
        };
    }

    public static string Serialize(object domainEvent, string? contentType = "application/json")
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        if (domainEvent is Howestprime.Movies.Domain.Events.MovieRegistered mr)
        {
            var message = new
            {
                id = mr.MovieId,
                title = mr.Title,
                year = mr.Year,
                duration = mr.Duration,
                genres = new[] { mr.Genre },
                actors = mr.Actors,
                ageRating = mr.AgeRating,
                poster = mr.PosterUrl
            };

            return JsonSerializer.Serialize(message, options);
        }

        if (domainEvent is Howestprime.Movies.Domain.Events.BookingOpened bo)
        {
            var seats = bo.SeatNumbers.Select(n => new { room = bo.RoomName, number = n, row = n });

            var message = new
            {
                bookingId = bo.BookingId,
                movieId = bo.MovieId,
                room = bo.RoomName,
                time = bo.Time,
                standardVisitors = bo.StandardVisitors,
                discountedVisitors = bo.DiscountVisitors,
                seats = seats
            };

            return JsonSerializer.Serialize(message, options);
        }


        return contentType switch
        {
            "application/json" => JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), options),
            _ => throw new NotImplementedException()
        };
    }

    private static Type ParseString<Type>(string message)
    {
        try
        {
            Type type = (Type)Convert.ChangeType(message, typeof(Type));

            return type;
        }
        catch (Exception ex)
        {
            throw new InvalidCastException(string.Format("Failed to parse string message {0} to type {1}", message, typeof(Type)), ex);
        }
    }

    public static JsonElement ParseJson(string message)
    {
        try
        {
            JsonElement json = JsonDocument.Parse(message).RootElement;

            if (json.ValueKind == JsonValueKind.Undefined)
                throw new InvalidCastException(string.Format("message body is empty: {0}, type: {1}", message, typeof(JsonElement)));
                
           return json;
        }
        catch (Exception ex)
        {
            throw new InvalidCastException(string.Format("Failed to parse json message {0} to type {1}", message, typeof(JsonElement)), ex);
        }
    }
    
    private static Type ParseJson<Type>(string message)
    {
        try
        {
            Type type = JsonSerializer.Deserialize<Type>(message)!
                ?? throw new InvalidCastException(string.Format("message body is empty: {0}, type: {1}", message, typeof(Type)));

            return type;
        }
        catch (Exception ex)
        {
            throw new InvalidCastException(string.Format("Failed to parse json message {0} to type {1}", message, typeof(Type)), ex);
        }

    }

}
