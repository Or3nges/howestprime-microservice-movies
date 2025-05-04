using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Howestprime.Movies.Application.Movies.FindMoviesWithEventsInTimeframe;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers
{
    [ApiController]
    [Route("api/movie-events")]
    public class FindMoviesWithEventsInTimeframeController : ControllerBase
    {
        private readonly FindMoviesWithEventsInTimeframeUseCase _useCase;
        public FindMoviesWithEventsInTimeframeController(FindMoviesWithEventsInTimeframeUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? Title, [FromQuery] string? Genre)
        {
            var query = new FindMoviesWithEventsInTimeframeQuery { Title = Title, Genre = Genre };
            var result = await _useCase.ExecuteAsync(query);
            return Ok(result);
        }
    }
}
