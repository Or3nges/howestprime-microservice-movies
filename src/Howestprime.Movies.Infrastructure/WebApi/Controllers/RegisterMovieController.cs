namespace Howestprime.Movies.Infrastructure.WebApi.Controllers;

using Howestprime.Movies.Application;
using Howestprime.Movies.Application.Movies.RegisterMovie;
using Howestprime.Movies.Application.DTO;
using Microsoft.AspNetCore.Http;

public static class RegisterMovieController
{
    public static async Task<IResult> Invoke(
        RegisterMovieRequest request,
        RegisterMovieUseCase useCase)
    {
        var command = new RegisterMovieCommand
        {
            Title = request.Title!,
            Description = request.Description,
            Genre = request.Genre,
            Actors = request.Actors,
            AgeRating = request.AgeRating,
            Duration = request.Duration,
            PosterUrl = request.PosterUrl
        };
        var movie = await useCase.ExecuteAsync(command);
        return Results.Created($"/api/movies/{movie.Id}", movie);
    }
}