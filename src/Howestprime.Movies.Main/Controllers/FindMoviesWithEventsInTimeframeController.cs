using System;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Movies.FindMoviesWithEventsInTimeframe;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Main.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FindMoviesWithEventsInTimeframeController : ControllerBase
    {
        private readonly FindMoviesWithEventsInTimeframeUseCase _useCase;

        public FindMoviesWithEventsInTimeframeController(FindMoviesWithEventsInTimeframeUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet]
        public async Task<ActionResult<MoviesWithEventsResponse>> GetMoviesWithEventsInTimeframe(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var query = new FindMoviesWithEventsInTimeframeQuery
            {
                StartDate = startDate,
                EndDate = endDate
            };

            var result = await _useCase.ExecuteAsync(query);
            return Ok(result);
        }
    }
} 