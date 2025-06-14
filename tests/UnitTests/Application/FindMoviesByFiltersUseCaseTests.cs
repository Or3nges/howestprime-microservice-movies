using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Howestprime.Movies.Application.Movies.FindMoviesByFilters;
using Howestprime.Movies.Domain.Movie;
using UnitTests.Shared;

namespace UnitTests.Application
{
    public class FindMoviesByFiltersUseCaseTests
    {
        private readonly FakeMovieRepository _movieRepository;
        private readonly FindMoviesByFiltersUseCase _useCase;

        public FindMoviesByFiltersUseCaseTests()
        {
            _movieRepository = new FakeMovieRepository();
            _useCase = new FindMoviesByFiltersUseCase(_movieRepository);
        }

        [Fact]
        public async Task ExecuteAsync_WithNullQuery_ReturnsEmptyList()
        {
            // Act
            var result = await _useCase.ExecuteAsync(null!);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task ExecuteAsync_WithNoFilters_ReturnsAllMovies()
        {
            // Arrange
            var movie1 = Movie.Create(
                "Inception",
                "A mind-bending thriller",
                2010,
                148,
                "Sci-Fi",
                "Leonardo DiCaprio",
                "12",
                "poster1.jpg"
            );

            var movie2 = Movie.Create(
                "The Matrix",
                "Virtual reality action",
                1999,
                136,
                "Sci-Fi",
                "Keanu Reeves",
                "16",
                "poster2.jpg"
            );

            await _movieRepository.Save(movie1);
            await _movieRepository.Save(movie2);

            var query = new FindMoviesByFiltersQuery();

            // Act
            var result = await _useCase.ExecuteAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Data.Count());
            Assert.Contains(result.Data, m => m.Title == "Inception");
            Assert.Contains(result.Data, m => m.Title == "The Matrix");
        }

        [Fact]
        public async Task ExecuteAsync_WithTitleFilter_ReturnsMatchingMovies()
        {
            // Arrange
            var movie1 = Movie.Create(
                "Inception",
                "A mind-bending thriller",
                2010,
                148,
                "Sci-Fi",
                "Leonardo DiCaprio",
                "12",
                "poster1.jpg"
            );

            var movie2 = Movie.Create(
                "The Matrix",
                "Virtual reality action",
                1999,
                136,
                "Sci-Fi",
                "Keanu Reeves",
                "16",
                "poster2.jpg"
            );

            await _movieRepository.Save(movie1);
            await _movieRepository.Save(movie2);

            var query = new FindMoviesByFiltersQuery { Title = "matrix" };

            // Act
            var result = await _useCase.ExecuteAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Data);
            Assert.Equal("The Matrix", result.Data.First().Title);
        }

        [Fact]
        public async Task ExecuteAsync_WithGenreFilter_ReturnsMatchingMovies()
        {
            // Arrange
            var movie1 = Movie.Create(
                "Inception",
                "A mind-bending thriller",
                2010,
                148,
                "Sci-Fi",
                "Leonardo DiCaprio",
                "12",
                "poster1.jpg"
            );

            var movie2 = Movie.Create(
                "Titanic",
                "A love story",
                1997,
                195,
                "Drama",
                "Leonardo DiCaprio",
                "12",
                "poster2.jpg"
            );

            await _movieRepository.Save(movie1);
            await _movieRepository.Save(movie2);

            var query = new FindMoviesByFiltersQuery { Genre = "Drama" };

            // Act
            var result = await _useCase.ExecuteAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Data);
            Assert.Equal("Titanic", result.Data.First().Title);
        }

        [Fact]
        public async Task ExecuteAsync_WithTitleAndGenreFilters_ReturnsMatchingMovies()
        {
            // Arrange
            var movie1 = Movie.Create(
                "Inception",
                "A mind-bending thriller",
                2010,
                148,
                "Sci-Fi",
                "Leonardo DiCaprio",
                "12",
                "poster1.jpg"
            );

            var movie2 = Movie.Create(
                "The Matrix",
                "Virtual reality action",
                1999,
                136,
                "Sci-Fi",
                "Keanu Reeves",
                "16",
                "poster2.jpg"
            );

            var movie3 = Movie.Create(
                "The Matrix Reloaded",
                "The saga continues",
                2003,
                138,
                "Action",
                "Keanu Reeves",
                "16",
                "poster3.jpg"
            );

            await _movieRepository.Save(movie1);
            await _movieRepository.Save(movie2);
            await _movieRepository.Save(movie3);

            var query = new FindMoviesByFiltersQuery { Title = "matrix", Genre = "Sci-Fi" };

            // Act
            var result = await _useCase.ExecuteAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Data);
            var movie = result.Data.First();
            Assert.Equal("The Matrix", movie.Title);
            Assert.Equal("Sci-Fi", movie.Genre);
        }

        [Fact]
        public async Task ExecuteAsync_WithNoMatches_ReturnsEmptyList()
        {
            // Arrange
            var movie1 = Movie.Create(
                "Inception",
                "A mind-bending thriller",
                2010,
                148,
                "Sci-Fi",
                "Leonardo DiCaprio",
                "12",
                "poster1.jpg"
            );

            await _movieRepository.Save(movie1);

            var query = new FindMoviesByFiltersQuery { Title = "nonexistent", Genre = "Fantasy" };

            // Act
            var result = await _useCase.ExecuteAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Data);
        }
    }
}
