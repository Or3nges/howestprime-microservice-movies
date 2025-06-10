namespace Howestprime.Movies.Infrastructure.WebApi.Controllers;

using Howestprime.Movies.Application.Movies.ScheduleMovieEvent;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

public static class ScheduleMovieEventController
{
    public static async Task<IResult> Invoke(
        ScheduleMovieEventCommand command,
        ScheduleMovieEventUseCase useCase)
    {
        try
        {
            // Validate required fields
            if (command.MovieId == Guid.Empty || command.RoomId == Guid.Empty)
            {
                return Results.BadRequest(new { error = "MovieId and RoomId are required" });
            }

            var result = await useCase.ExecuteAsync(command);
            return Results.Created($"/api/movie-events/{result.EventId}", result);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }
}
