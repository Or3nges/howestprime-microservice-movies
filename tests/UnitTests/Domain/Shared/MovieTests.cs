using System;
using Howestprime.Movies.Domain.Shared;
using Xunit;

namespace UnitTests.Domain.Shared
{
    public class MovieTests
    {
        [Fact]
        public void Movie_CanBeCreated_WithValidData()
        {
            var id = Guid.NewGuid();
            var movie = new Movie(id, "Test", "Desc", "Action", "Actor", "PG", 120, "url");
            Assert.Equal(id, movie.Id);
            Assert.Equal("Test", movie.Title);
            Assert.Equal("Desc", movie.Description);
            Assert.Equal("Action", movie.Genres);
            Assert.Equal("Actor", movie.Actors);
            Assert.Equal("PG", movie.AgeRating);
            Assert.Equal(120, movie.Duration);
            Assert.Equal("url", movie.PosterUrl);
        }
    }
}
