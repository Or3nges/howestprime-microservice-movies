using System;
using Howestprime.Movies.Domain.Movie;
using Xunit;

namespace UnitTests.Domain
{
    public class MovieTests
    {
        [Fact]
        public void CreateMovie_Succeeds()
        {
            var movie = Movie.Create("Title", "Description", 2022, 120, "Genre", "Actors", "PG-13", "poster.jpg", new MovieId());

            Assert.NotNull(movie);
            Assert.Equal("Title", movie.Title);
            Assert.Equal("Description", movie.Description);
            Assert.Equal(2022, movie.Year);
            Assert.Equal(120, movie.Duration);
            Assert.Equal("Genre", movie.Genre);
            Assert.Equal("Actors", movie.Actors);
            Assert.Equal("poster.jpg", movie.PosterUrl);
        }

        [Fact]
        public void CreateMovie_WithInvalidYear_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => Movie.Create("Title", "Description", 1800, 120, "Genre", "Actors", "PG-13", "poster.jpg", new MovieId()));
        }

        [Fact]
        public void CreateMovie_WithInvalidDuration_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => Movie.Create("Title", "Description", 2024, -1, "Genre", "Actors", "PG-13", "poster.jpg", new MovieId()));
        }

        [Fact]
        public void CreateMovie_WithEmptyTitle_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => Movie.Create("", "Description", 2024, 120, "Genre", "Actors", "PG-13", "poster.jpg", new MovieId()));
        }

        [Fact]
        public void CreateMovie_WithEmptyDescription_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => Movie.Create("Title", "", 2024, 120, "Genre", "Actors", "PG-13", "poster.jpg", new MovieId()));
        }

        [Fact]
        public void CreateMovie_WithEmptyGenre_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => Movie.Create("Title", "Description", 2024, 120, "", "Actors", "PG-13", "poster.jpg", new MovieId()));
        }

        [Fact]
        public void CreateMovie_WithEmptyActors_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => Movie.Create("Title", "Description", 2024, 120, "Genre", "", "PG-13", "poster.jpg", new MovieId()));
        }

        [Fact]
        public void CreateMovie_WithEmptyPosterUrl_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => Movie.Create("Title", "Description", 2024, 120, "Genre", "Actors", "PG-13", "", new MovieId()));
        }
    }
}
