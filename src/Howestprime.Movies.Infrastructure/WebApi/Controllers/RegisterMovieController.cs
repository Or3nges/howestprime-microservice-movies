namespace Howestprime.Movies.Infrastructure.WebApi.Controllers;

using Howestprime.Movies.Application;
using Howestprime.Movies.Application.Movies.RegisterMovie;
using Howestprime.Movies.Domain.Movie;
using Microsoft.AspNetCore.Http;

public static class RegisterMovieController
{
    public static async Task<IResult> Invoke(
        RegisterMovieCommand command,
        RegisterMovieUseCase useCase)
    {
        try
        {
            var result = await useCase.ExecuteAsync(command);
            return Results.Created($"/api/movies/{result.Id}", result);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }
}