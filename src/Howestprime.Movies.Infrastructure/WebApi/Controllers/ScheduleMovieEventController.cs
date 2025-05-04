using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Howestprime.Movies.Application.Movies.ScheduleMovieEvent;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers
{
    [ApiController]
    [Route("api/movie-events")]
    public class ScheduleMovieEventController : ControllerBase
    {
        private readonly ScheduleMovieEventUseCase _useCase;
        public ScheduleMovieEventController(ScheduleMovieEventUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost]
        public async Task<IActionResult> Schedule([FromBody] ScheduleMovieEventCommand command)
        {
            try
            {
                var result = await _useCase.ExecuteAsync(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
