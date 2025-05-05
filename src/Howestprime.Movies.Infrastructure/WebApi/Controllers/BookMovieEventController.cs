using Howestprime.Movies.Application.Movies.BookMovieEvent;
using Microsoft.AspNetCore.Http;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers;

public static class BookMovieEventController
{
    public static async Task<IResult> Invoke(
        BookMovieEventCommand command,
        BookMovieEventHandler handler)
    {
        var result = await handler.HandleAsync(command);
        return Results.Ok(result);
    }
}
