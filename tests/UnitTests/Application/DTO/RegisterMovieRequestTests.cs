using System;
using Howestprime.Movies.Application.DTO;
using Howestprime.Movies.Application.Movies.RegisterMovie;
using Xunit;

namespace UnitTests.Application.DTO
{
    public class RegisterMovieRequestTests
    {
        [Fact]
        public void RegisterMovieRequest_CanBeCreated_WithValidData()
        {
            var request = new RegisterMovieRequest
            {
                Title = "Test Movie",
                Description = "A test movie",
                Genre = "Action",
                Actors = "Actor 1, Actor 2",
                AgeRating = 12,
                Duration = 120,
                PosterUrl = "http://example.com/poster.jpg"
            };
            Assert.Equal("Test Movie", request.Title);
            Assert.Equal("A test movie", request.Description);
            Assert.Equal("Action", request.Genre);
            Assert.Equal("Actor 1, Actor 2", request.Actors);
            Assert.Equal(12, request.AgeRating);
            Assert.Equal(120, request.Duration);
            Assert.Equal("http://example.com/poster.jpg", request.PosterUrl);
        }

        [Fact]
        public void RegisterMovieRequest_ToCommand_MapsCorrectly()
        {
            var request = new RegisterMovieRequest
            {
                Title = "Test Movie",
                Description = "A test movie",
                Genre = "Action",
                Actors = "Actor 1, Actor 2",
                AgeRating = 12,
                Duration = 120,
                PosterUrl = "http://example.com/poster.jpg"
            };
            var command = new RegisterMovieCommand
            {
                Title = request.Title!,
                Description = request.Description,
                Genre = request.Genre,
                Actors = request.Actors,
                AgeRating = request.AgeRating,
                Duration = request.Duration,
                PosterUrl = request.PosterUrl
            };
            Assert.Equal(request.Title, command.Title);
            Assert.Equal(request.Description, command.Description);
            Assert.Equal(request.Genre, command.Genre);
            Assert.Equal(request.Actors, command.Actors);
            Assert.Equal(request.AgeRating, command.AgeRating);
            Assert.Equal(request.Duration, command.Duration);
            Assert.Equal(request.PosterUrl, command.PosterUrl);
        }
    }
}
