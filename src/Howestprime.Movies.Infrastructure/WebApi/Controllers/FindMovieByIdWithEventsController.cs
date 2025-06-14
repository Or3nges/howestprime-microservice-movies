namespace Howestprime.Movies.Infrastructure.WebApi.Controllers;

using Howestprime.Movies.Application.Movies.FindMovieByIdWithEvents;
using Howestprime.Movies.Domain.Movie;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public static class FindMovieByIdWithEventsController
{    // This method handles query parameter approach as per webapi.yaml specification
    public static async Task<IResult> Invoke(
        [FromQuery] string movieId,
        FindMovieByIdWithEventsUseCase useCase)
    {
        try
        {
            Console.WriteLine("start try");
            var query = new FindMovieByIdWithEventsQuery { MovieId = new MovieId(movieId) };
            Console.WriteLine("query done");
            var result = await useCase.ExecuteAsync(query);
            Console.WriteLine("result. ", result);
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }
}
