using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Howestprime.Movies.Application.Movies.FindMovieByIdWithEvents;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers
{
    [ApiController]
    [Route("api/movie-events/movie")]
    public class FindMovieByIdWithEventsController : ControllerBase
    {
        private readonly FindMovieByIdWithEventsUseCase _useCase;
        public FindMovieByIdWithEventsController(FindMovieByIdWithEventsUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid movieId)
        {
            try
            {
                var query = new FindMovieByIdWithEventsQuery { MovieId = movieId };
                var result = await _useCase.ExecuteAsync(query);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
