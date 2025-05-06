using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Entities;

namespace Howestprime.Movies.Application.Movies.FindMoviesByFilters
{
    public class FindMoviesByFiltersUseCase
    {
        private readonly IMovieRepository _movieRepository;

        public FindMoviesByFiltersUseCase(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<Movie>> ExecuteAsync(FindMoviesByFiltersQuery query)
        {
            if (query == null)
                return Enumerable.Empty<Movie>();

            return await _movieRepository.FindByFiltersAsync(query.Title, query.Genre);
        }
    }
}
