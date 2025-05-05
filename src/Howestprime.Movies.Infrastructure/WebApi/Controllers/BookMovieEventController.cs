using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Movies.BookMovieEvent;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookMovieEventController : ControllerBase
    {
        private readonly BookMovieEventHandler _handler;

        public BookMovieEventController(BookMovieEventHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        public async Task<IActionResult> Book([FromBody] BookMovieEventCommand command)
        {
            var result = await _handler.HandleAsync(command);
            return Ok(result);
        }
    }
}
