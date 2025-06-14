namespace Howestprime.Movies.Infrastructure.WebApi.Controllers.MovieEvents;

using Howestprime.Movies.Application.Movies.FindMoviesWithEventsInTimeframe;
using Microsoft.AspNetCore.Http;

public static class FindMovieWithEventsInTimeFrameController
{
    public static async Task<IResult> Invoke(
        [AsParameters] FindMoviesWithEventsInTimeframeQuery query,
        FindMoviesWithEventsInTimeframeUseCase useCase)
    {
        try
        {
            var result = await useCase.ExecuteAsync(query);
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }
}
