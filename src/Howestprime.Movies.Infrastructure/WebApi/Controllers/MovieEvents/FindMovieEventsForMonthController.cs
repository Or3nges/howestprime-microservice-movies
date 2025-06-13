using Howestprime.Movies.Application.Movies.FindMovieEventsForMonth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Howestprime.Movies.Domain.Entities;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers;

public static class FindMovieEventsForMonthController
{
    public static async Task<IResult> Invoke(
        [FromQuery] int year,
        [FromQuery] int month,
        FindMovieEventsForMonthUseCase useCase)
    {
        try
        {
            var query = new FindMovieEventsForMonthQuery 
            { 
                Year = year,
                Month = month
            };
            var result = await useCase.ExecuteAsync(query);
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }
}
