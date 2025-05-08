using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies.FindMoviesByFilters;
using Howestprime.Movies.Domain.Entities;
using Xunit;

namespace UnitTests.Application
{
    public class FindMoviesByFiltersUseCaseTests
    {
        private class FakeMovieRepository : IMovieRepository
        {
            public IEnumerable<Movie> Movies = new List<Movie>();
            public Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult((Movie?)null);
            public Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken = default) => throw new NotImplementedException();
            public Task<IEnumerable<Movie>> FindByFiltersAsync(string? title, string? genre, CancellationToken cancellationToken = default) => Task.FromResult(Movies);
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsMovies_WhenFound()
        {
            var movie = new Movie(Guid.NewGuid(), "Title", "Desc", "Genre", "Actors", "PG", 120, "url");
            var movies = new List<Movie> { movie };
            var useCase = new FindMoviesByFiltersUseCase(new FakeMovieRepository { Movies = movies });
            var query = new FindMoviesByFiltersQuery { Title = "Title", Genre = "Genre" };
            var result = await useCase.ExecuteAsync(query);
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsEmpty_WhenNoMoviesFound()
        {
            var useCase = new FindMoviesByFiltersUseCase(new FakeMovieRepository { Movies = new List<Movie>() });
            var query = new FindMoviesByFiltersQuery { Title = "None", Genre = "None" };
            var result = await useCase.ExecuteAsync(query);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task ExecuteAsync_NullQuery_ReturnsEmpty()
        {
            var useCase = new FindMoviesByFiltersUseCase(new FakeMovieRepository { Movies = new List<Movie>() });
            var result = await useCase.ExecuteAsync(null);
            Assert.Empty(result.Data);
        }
    }
}
