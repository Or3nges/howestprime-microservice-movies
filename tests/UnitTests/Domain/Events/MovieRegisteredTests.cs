using System;
using Howestprime.Movies.Domain.Events;
using Howestprime.Movies.Domain.Movie;
using Xunit;

namespace UnitTests.Domain.Events
{
    public class MovieRegisteredTests
    {
        [Fact]
        public void Create_Succeeds()
        {
            var movieId = new MovieId();
            var title = "Test Movie";
            var year = 2024;
            var duration = 120;
            var genre = "Action";
            var actors = "Actor 1, Actor 2";
            var ageRating = 12;
            var posterUrl = "http://example.com/poster.jpg";

            var movieRegistered = MovieRegistered.Create(
                movieId.Value,
                title,
                year,
                duration,
                genre,
                actors,
                ageRating,
                posterUrl
            );

            Assert.Equal(movieId.Value, movieRegistered.MovieId);
            Assert.Equal(title, movieRegistered.Title);
            Assert.Equal(year, movieRegistered.Year);
            Assert.Equal(duration, movieRegistered.Duration);
            Assert.Equal(genre, movieRegistered.Genre);
            Assert.Equal(actors, movieRegistered.Actors);
            Assert.Equal(ageRating, movieRegistered.AgeRating);
            Assert.Equal(posterUrl, movieRegistered.PosterUrl);
            Assert.Equal("MovieRegistered", movieRegistered.QualifiedEventName);
            Assert.True(movieRegistered.OccurredOn <= DateTime.UtcNow);
        }
    }
} 