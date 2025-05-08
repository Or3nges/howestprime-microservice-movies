namespace Howestprime.Movies.Infrastructure.WebApi.Controllers;

using Howestprime.Movies.Application.Movies.FindMovieByIdWithEvents;
using Microsoft.AspNetCore.Http;

public static class FindMovieByIdWithEventsController
{
    public static async Task<IResult> Invoke(
        [AsParameters] FindMovieByIdWithEventsQuery query,
        FindMovieByIdWithEventsUseCase useCase)
    {
        var result = await useCase.ExecuteAsync(query);
        return Results.Ok(result);
    }
}
