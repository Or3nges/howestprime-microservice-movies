using System.Collections.Generic;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.MovieEvent;
using Howestprime.Movies.Application.Contracts.Data;

namespace Howestprime.Movies.Application.Movies.FindMovieEventsForMonth
{
    public class FindMovieEventsForMonthQueryHandler
    {
        private readonly FindMovieEventsForMonthUseCase _useCase;

        public FindMovieEventsForMonthQueryHandler(FindMovieEventsForMonthUseCase useCase)
        {
            _useCase = useCase;
        }

        public async Task<List<MovieEventData>> HandleAsync(FindMovieEventsForMonthQuery query)
        {
            return await _useCase.ExecuteAsync(query);
        }
    }
} 