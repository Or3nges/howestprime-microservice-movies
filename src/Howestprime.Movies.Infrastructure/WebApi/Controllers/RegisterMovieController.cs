using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Howestprime.Movies.Application;
using Howestprime.Movies.Application.DTO;
using Howestprime.Movies.Application.Movies.RegisterMovie;

namespace Howestprime.Movies.Infrastructure.WebApi.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class RegisterMovieController : ControllerBase
    {
        private readonly RegisterMovieUseCase _registerMovieUseCase;

        public RegisterMovieController(RegisterMovieUseCase registerMovieUseCase)
        {
            _registerMovieUseCase = registerMovieUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterMovieRequest request)
        {
            var command = new RegisterMovieCommand
            {
                Title = request.Title!,
                Description = request.Description,
                Genre = request.Genre,
                Actors = request.Actors,
                AgeRating = request.AgeRating,
                Duration = request.Duration,
                PosterUrl = request.PosterUrl
            };

            var movie = await _registerMovieUseCase.ExecuteAsync(command);
            return Ok(movie);
        }
    }
}