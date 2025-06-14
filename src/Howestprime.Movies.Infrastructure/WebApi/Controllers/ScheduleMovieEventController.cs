namespace Howestprime.Movies.Infrastructure.WebApi.Controllers;

using Howestprime.Movies.Application.Movies.ScheduleMovieEvent;
using Howestprime.Movies.Domain.MovieEvent;
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
            if (string.IsNullOrEmpty(command.MovieId) || string.IsNullOrEmpty(command.RoomId))
            {
                return Results.BadRequest(new { error = "MovieId and RoomId are required" });
            }

            await useCase.ExecuteAsync(command);
            return Results.Created($"/api/movie-events/{command.MovieId}", new { message = "Movie event scheduled successfully" });
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }
}
