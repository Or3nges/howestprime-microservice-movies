using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Howestprime.Movies.Application.Movies.FindMovieById;
using Howestprime.Movies.Domain.Shared;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class FindMovieByIdController : ControllerBase
    {
        private readonly FindMovieByIdUseCase _useCase;
        public FindMovieByIdController(FindMovieByIdUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var query = new MovieByIdQuery { Id = id };
                MovieData movie = await _useCase.ExecuteAsync(query);
                return Ok(movie);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
