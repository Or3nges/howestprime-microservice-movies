namespace Howestprime.Movies.Infrastructure.WebApi.Controllers;

using Howestprime.Movies.Application.Movies.FindMoviesByFilters;
using Microsoft.AspNetCore.Http;

public static class FindMoviesByFiltersController
{
    public static async Task<IResult> Invoke(
        [AsParameters] FindMoviesByFiltersQuery query,
        FindMoviesByFiltersUseCase useCase)
    {
        var movies = await useCase.ExecuteAsync(query);
        return Results.Ok(movies);
    }
}
