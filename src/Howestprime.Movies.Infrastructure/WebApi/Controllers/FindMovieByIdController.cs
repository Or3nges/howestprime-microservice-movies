namespace Howestprime.Movies.Infrastructure.WebApi.Controllers;

using Howestprime.Movies.Application.Movies.FindMovieById;
using Microsoft.AspNetCore.Http;

public static class FindMovieByIdController
{
    public static async Task<IResult> Invoke(
        Guid id,
        FindMovieByIdUseCase useCase)
    {
        var query = new MovieByIdQuery { Id = new Howestprime.Movies.Domain.Entities.MovieId(id.ToString()) };
        var movie = await useCase.ExecuteAsync(query);
        return movie != null ? Results.Ok(movie) : Results.NotFound();
    }
}
