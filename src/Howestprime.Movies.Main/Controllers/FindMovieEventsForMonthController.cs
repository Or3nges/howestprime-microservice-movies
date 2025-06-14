using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Howestprime.Movies.Application.Movies.FindMovieEventsForMonth;
using Howestprime.Movies.Application.Contracts.Data;
using Microsoft.AspNetCore.Mvc;

namespace Howestprime.Movies.Main.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FindMovieEventsForMonthController : ControllerBase
    {
        private readonly FindMovieEventsForMonthQueryHandler _handler;

        public FindMovieEventsForMonthController(FindMovieEventsForMonthQueryHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieEventData>>> GetMovieEventsForMonth(
            [FromQuery] int month,
            [FromQuery] int year)
        {
            var query = new FindMovieEventsForMonthQuery
            {
                Month = month,
                Year = year
            };

            var result = await _handler.HandleAsync(query);
            return Ok(result);
        }
    }
} 