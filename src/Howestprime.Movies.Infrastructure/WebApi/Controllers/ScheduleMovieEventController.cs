namespace Howestprime.Movies.Infrastructure.WebApi.Controllers;

using Howestprime.Movies.Application.Movies.ScheduleMovieEvent;
using Microsoft.AspNetCore.Http;

public static class ScheduleMovieEventController
{
    public static async Task<IResult> Invoke(
        ScheduleMovieEventCommand command,
        ScheduleMovieEventUseCase useCase)
    {
        var result = await useCase.ExecuteAsync(command);
        return Results.Created($"/api/movie-events/{result.EventId}", result);
    }
}
