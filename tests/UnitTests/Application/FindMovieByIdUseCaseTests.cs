using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Movies.FindMovieById;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Domain.Shared.Exceptions;
using Xunit;

namespace UnitTests.Application
{
    public class FakeMovieRepository : IMovieRepository
    {
        private readonly Dictionary<MovieId, Movie> _movies = new();
        public IReadOnlyDictionary<MovieId, Movie> Movies => _movies;

        public Task Save(Movie entity)
        {
            _movies[entity.Id] = entity;
            return Task.CompletedTask;
        }

        public Task<Movie?> GetByIdAsync(MovieId id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_movies.GetValueOrDefault(id));
        }

        public Task<IEnumerable<Movie>> FindByFiltersAsync(string? title, string? genre, CancellationToken cancellationToken = default)
        {
            var query = _movies.Values.AsEnumerable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(m => m.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(genre))
                query = query.Where(m => m.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(query);
        }

        public Task<Movie?> ById(MovieId id)
        {
            return Task.FromResult(_movies.GetValueOrDefault(id));
        }

        public Task Remove(Movie movie)
        {
            _movies.Remove(movie.Id);
            return Task.CompletedTask;
        }

        public Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken = default)
        {
            _movies[movie.Id] = movie;
            return Task.FromResult(movie);
        }
    }

    public class FindMovieByIdUseCaseTests
    {
        private readonly FakeMovieRepository _movieRepository;
        private readonly FindMovieByIdUseCase _useCase;

        public FindMovieByIdUseCaseTests()
        {
            _movieRepository = new FakeMovieRepository();
            _useCase = new FindMovieByIdUseCase(_movieRepository);
        }

        [Fact]
        public async Task ExecuteAsync_ExistingMovie_ReturnsMovieData()
        {
            // Arrange
            var movieId = new MovieId();
            var movie = new Movie(movieId, "Test Movie", "Description", 2024, "Action", "Actor 1, Actor 2", "12", 120, "http://example.com/poster.jpg");
            await _movieRepository.AddAsync(movie);

            var query = new MovieByIdQuery { Id = movieId };

            // Act
            var result = await _useCase.ExecuteAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(movieId.Value, result.Id);
            Assert.Equal("Test Movie", result.Title);
            Assert.Equal("Description", result.Description);
            Assert.Equal(2024, result.Year);
            Assert.Equal(120, result.Duration);
            Assert.Equal("Action", result.Genre);
            Assert.Equal("Actor 1, Actor 2", result.Actors);
            Assert.Equal("http://example.com/poster.jpg", result.PosterUrl);
        }

        [Fact]
        public async Task ExecuteAsync_NonExistingMovie_ThrowsNotFoundException()
        {
            // Arrange
            var query = new MovieByIdQuery { Id = new MovieId() };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _useCase.ExecuteAsync(query));
        }
    }
}
