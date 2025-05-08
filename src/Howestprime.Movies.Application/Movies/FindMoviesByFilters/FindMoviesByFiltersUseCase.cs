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

        public async Task<MoviesResponse> ExecuteAsync(FindMoviesByFiltersQuery query)
        {
            if (query == null)
                return new MoviesResponse { Data = Enumerable.Empty<Movie>() };

            var movies = await _movieRepository.FindByFiltersAsync(query.Title, query.Genre);
            return new MoviesResponse { Data = movies };
        }
    }

    public class MoviesResponse
    {
        public IEnumerable<Movie> Data { get; set; } = Enumerable.Empty<Movie>();
    }
}
