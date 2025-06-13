using System;
using Howestprime.Movies.Domain.Entities;
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
        }

        [Fact]
        public void CreateMovie_WithInvalidYear_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => Movie.Create("Title", "Description", 1800, 120, "Genre", "Actors", "PG-13", "poster.jpg", new MovieId()));
        }
    }
}
