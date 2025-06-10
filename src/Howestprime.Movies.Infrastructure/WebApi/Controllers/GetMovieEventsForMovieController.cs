using Howestprime.Movies.Application.Movies.FindMovieByIdWithEvents;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers
{
    public static class GetMovieEventsForMovieController
    {
        public static async Task<IResult> Invoke(
            Guid movieId,
            FindMovieByIdWithEventsUseCase useCase)
        {
            try
            {
                var query = new FindMovieByIdWithEventsQuery { MovieId = movieId };
                var result = await useCase.ExecuteAsync(query);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }
    }
}
