using Howestprime.Movies.Application.Movies.FindMovieEventsForMonth;
using Microsoft.AspNetCore.Http;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers;

public static class FindMovieEventsForMonthController
{
    public static async Task<IResult> Invoke(
        [AsParameters] FindMovieEventsForMonthQuery query,
        FindMovieEventsForMonthUseCase useCase)
    {
        var result = await useCase.ExecuteAsync(query);
        return Results.Ok(result);
    }
}
