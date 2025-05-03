using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Howestprime.Movies.Application.Movies.FindMoviesByFilters;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers
{
    [ApiController]
    [Route("api/movies/search")]
    public class FindMoviesByFiltersController : ControllerBase
    {
        private readonly FindMoviesByFiltersUseCase _useCase;

        public FindMoviesByFiltersController(FindMoviesByFiltersUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? title, [FromQuery] string? genre)
        {
            var query = new FindMoviesByFiltersQuery { Title = title, Genre = genre };
            var movies = await _useCase.ExecuteAsync(query);
            return Ok(movies);
        }
    }
}
