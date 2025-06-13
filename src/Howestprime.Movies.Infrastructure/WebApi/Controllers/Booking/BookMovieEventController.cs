using Howestprime.Movies.Application.Movies.BookMovieEvent;
using Microsoft.AspNetCore.Http;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.Booking;

public static class BookMovieEventController
{
    public static async Task<IResult> Invoke(
        Guid movieEventId,
        BookMovieEventCommand command,
        BookMovieEventHandler handler)
    {
        command.MovieEventId = new Howestprime.Movies.Domain.Entities.MovieEventId(movieEventId.ToString());
        
        var result = await handler.HandleAsync(command);
        
        return Results.Created($"/api/movie-events/{movieEventId}/bookings/{result.BookingId}", result);
    }
}
