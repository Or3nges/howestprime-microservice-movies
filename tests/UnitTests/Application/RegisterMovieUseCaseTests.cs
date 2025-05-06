using System;
using System.Threading.Tasks;
using Howestprime.Movies.Application;
using Howestprime.Movies.Application.Movies.RegisterMovie;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Entities;
using Xunit;
using System.Collections.Generic;
using System.Threading;

namespace UnitTests.Application
{
    public class RegisterMovieUseCaseTests
    {
        private class FakeMovieRepository : IMovieRepository
        {
            public Movie? AddedMovie;
            public Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken = default)
            {
                AddedMovie = movie;
                return Task.FromResult(movie);
            }
            public Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => throw new NotImplementedException();
            public Task<IEnumerable<Movie>> FindByFiltersAsync(string? title, string? genre, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        }

        [Fact]
        public async Task ExecuteAsync_ValidCommand_ReturnsMovie()
        {
            // Arrange
            var fakeRepo = new FakeMovieRepository();
            var useCase = new RegisterMovieUseCase(fakeRepo);
            var command = new RegisterMovieCommand
            {
                Title = "Test Movie",
                Duration = 120
            };

            // Act
            var result = await useCase.ExecuteAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Movie", result.Title);
            Assert.Equal(120, result.Duration);
            Assert.Same(result, fakeRepo.AddedMovie);
        }

        [Fact]
        public async Task ExecuteAsync_EmptyTitle_ThrowsArgumentException()
        {
            var fakeRepo = new FakeMovieRepository();
            var useCase = new RegisterMovieUseCase(fakeRepo);
            var command = new RegisterMovieCommand { Title = "", Duration = 120 };
            await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(command));
        }

        [Fact]
        public async Task ExecuteAsync_NegativeDuration_ThrowsArgumentException()
        {
            var fakeRepo = new FakeMovieRepository();
            var useCase = new RegisterMovieUseCase(fakeRepo);
            var command = new RegisterMovieCommand { Title = "Test", Duration = -1 };
            await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(command));
        }

        [Fact]
        public async Task ExecuteAsync_NullDescription_AllowsMovieCreation()
        {
            var fakeRepo = new FakeMovieRepository();
            var useCase = new RegisterMovieUseCase(fakeRepo);
            var command = new RegisterMovieCommand { Title = "Test", Duration = 100, Description = null };
            var result = await useCase.ExecuteAsync(command);
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.Description);
        }

        [Fact]
        public async Task ExecuteAsync_NullGenre_AllowsMovieCreation()
        {
            var fakeRepo = new FakeMovieRepository();
            var useCase = new RegisterMovieUseCase(fakeRepo);
            var command = new RegisterMovieCommand { Title = "Test", Duration = 100, Genre = null };
            var result = await useCase.ExecuteAsync(command);
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.Genre);
        }

        [Fact]
        public async Task ExecuteAsync_NullActors_AllowsMovieCreation()
        {
            var fakeRepo = new FakeMovieRepository();
            var useCase = new RegisterMovieUseCase(fakeRepo);
            var command = new RegisterMovieCommand { Title = "Test", Duration = 100, Actors = null };
            var result = await useCase.ExecuteAsync(command);
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.Actors);
        }

        [Fact]
        public async Task ExecuteAsync_NullPosterUrl_AllowsMovieCreation()
        {
            var fakeRepo = new FakeMovieRepository();
            var useCase = new RegisterMovieUseCase(fakeRepo);
            var command = new RegisterMovieCommand { Title = "Test", Duration = 100, PosterUrl = null };
            var result = await useCase.ExecuteAsync(command);
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.PosterUrl);
        }

        [Fact]
        public async Task ExecuteAsync_NullTitle_ThrowsArgumentException()
        {
            var fakeRepo = new FakeMovieRepository();
            var useCase = new RegisterMovieUseCase(fakeRepo);
            var command = new RegisterMovieCommand { Title = null, Duration = 120 };
            await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(command));
        }

        [Fact]
        public async Task ExecuteAsync_ZeroDuration_ThrowsArgumentException()
        {
            var fakeRepo = new FakeMovieRepository();
            var useCase = new RegisterMovieUseCase(fakeRepo);
            var command = new RegisterMovieCommand { Title = "Test", Duration = 0 };
            await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(command));
        }
    }
}
