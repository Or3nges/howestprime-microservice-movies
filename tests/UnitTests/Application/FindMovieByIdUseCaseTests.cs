using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies.FindMovieById;
using Howestprime.Movies.Domain.Entities;
using Xunit;

namespace UnitTests.Application
{
    public class FindMovieByIdUseCaseTests
    {
        private class FakeMovieRepository : IMovieRepository
        {
            public Movie? Movie;
            public Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult(Movie);
            public Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken = default) => throw new NotImplementedException();
            public Task<IEnumerable<Movie>> FindByFiltersAsync(string? title, string? genre, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        }

        [Fact]
        public async Task ExecuteAsync_ReturnsMovie_WhenFound()
        {
            var movie = new Movie(Guid.NewGuid(), "Title", "Desc", "Genre", "Actors", "PG", 120, "url");
            var useCase = new FindMovieByIdUseCase(new FakeMovieRepository { Movie = movie });
            var query = new MovieByIdQuery { Id = movie.Id };
            var result = await useCase.ExecuteAsync(query);
            Assert.NotNull(result);
            Assert.Equal(movie.Id, result.Id);
        }

        [Fact]
        public async Task ExecuteAsync_Throws_WhenNotFound()
        {
            var useCase = new FindMovieByIdUseCase(new FakeMovieRepository { Movie = null });
            var query = new MovieByIdQuery { Id = Guid.NewGuid() };
            await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(query));
        }
    }
}
