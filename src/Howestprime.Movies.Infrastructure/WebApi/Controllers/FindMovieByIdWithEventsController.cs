namespace Howestprime.Movies.Infrastructure.WebApi.Controllers;

using Howestprime.Movies.Application.Movies.FindMovieByIdWithEvents;
using Microsoft.AspNetCore.Http;

public static class FindMovieByIdWithEventsController
{
    public static async Task<IResult> Invoke(
        Guid id,
        FindMovieByIdWithEventsUseCase useCase)
    {
        var query = new FindMovieByIdWithEventsQuery { MovieId = id };
        var result = await useCase.ExecuteAsync(query);
        return Results.Ok(result);
    }
}
