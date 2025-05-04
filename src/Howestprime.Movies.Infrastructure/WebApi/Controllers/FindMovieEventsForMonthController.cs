using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Howestprime.Movies.Application.Movies.FindMovieEventsForMonth;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers
{
    [ApiController]
    [Route("api/movie-events/monthly")]
    public class FindMovieEventsForMonthController : ControllerBase
    {
        private readonly FindMovieEventsForMonthUseCase _useCase;
        public FindMovieEventsForMonthController(FindMovieEventsForMonthUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int year, [FromQuery] int month)
        {
            var query = new FindMovieEventsForMonthQuery { Year = year, Month = month };
            var result = await _useCase.ExecuteAsync(query);
            return Ok(result);
        }
    }
}
