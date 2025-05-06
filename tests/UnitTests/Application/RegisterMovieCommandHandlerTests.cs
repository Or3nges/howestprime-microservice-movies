using System;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Movies.RegisterMovie;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Entities;
using Xunit;

namespace UnitTests.Application
{
    public class RegisterMovieCommandHandlerTests
    {
        private class FakeMovieRepository : IMovieRepository
        {
            public Movie? AddedMovie;
            public Task<Movie> AddAsync(Movie movie, System.Threading.CancellationToken cancellationToken = default)
            {
                AddedMovie = movie;
                return Task.FromResult(movie);
            }
            public Task<Movie?> GetByIdAsync(Guid id, System.Threading.CancellationToken cancellationToken = default) => throw new System.NotImplementedException();
            public Task<System.Collections.Generic.IEnumerable<Movie>> FindByFiltersAsync(string? title, string? genre, System.Threading.CancellationToken cancellationToken = default) => throw new System.NotImplementedException();
        }

        [Fact]
        public async Task Handle_ValidCommand_AddsMovie()
        {
            var repo = new FakeMovieRepository();
            var handler = new RegisterMovieCommandHandler(repo);
            var command = new RegisterMovieCommand { Title = "Test", Duration = 120 };
            var result = await handler.Handle(command);
            Assert.NotNull(result);
            Assert.Equal("Test", result.Title);
            Assert.Equal(120, result.Duration);
            Assert.Same(result, repo.AddedMovie);
        }
    }
}
