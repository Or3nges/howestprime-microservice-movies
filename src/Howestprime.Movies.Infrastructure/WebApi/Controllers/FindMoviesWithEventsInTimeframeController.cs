namespace Howestprime.Movies.Infrastructure.WebApi.Controllers;

using Howestprime.Movies.Application.Movies.FindMoviesWithEventsInTimeframe;
using Microsoft.AspNetCore.Http;

public static class FindMoviesWithEventsInTimeframeController
{
    public static async Task<IResult> Invoke(
        [AsParameters] FindMoviesWithEventsInTimeframeQuery query,
        FindMoviesWithEventsInTimeframeUseCase useCase)
    {
        var result = await useCase.ExecuteAsync(query);
        return Results.Ok(result);
    }
}
