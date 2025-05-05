using System;
using Howestprime.Movies.Domain.Entities;
using Xunit;

namespace UnitTests.Domain
{
    public class MovieTests
    {
        [Fact]
        public void Movie_CanBeCreated_WithValidData()
        {
            var id = Guid.NewGuid();
            var movie = new Movie(id, "Title", "Desc", "Genre", "Actors", "PG", 120, "url");
            Assert.Equal(id, movie.Id);
            Assert.Equal("Title", movie.Title);
            Assert.Equal("Desc", movie.Description);
            Assert.Equal("Genre", movie.Genre);
            Assert.Equal("Actors", movie.Actors);
            Assert.Equal("PG", movie.AgeRating);
            Assert.Equal(120, movie.Duration);
            Assert.Equal("url", movie.PosterUrl);
        }
    }
}
