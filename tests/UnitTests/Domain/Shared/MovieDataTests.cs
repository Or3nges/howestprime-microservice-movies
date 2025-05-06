using System;
using Howestprime.Movies.Domain.Shared;
using Xunit;
using System.Collections.Generic;

namespace UnitTests.Domain.Shared
{
    public class MovieDataTests
    {
        [Fact]
        public void MovieData_CanBeCreated_WithValidData()
        {
            var id = Guid.NewGuid();
            var events = new List<MovieEventData> { new MovieEventData { Id = Guid.NewGuid() } };
            var movieData = new MovieData
            {
                Id = id,
                Title = "Test Movie",
                Genres = "Action",
                Actors = "Actor 1, Actor 2",
                AgeRating = "PG",
                Duration = 120,
                PosterUrl = "http://example.com/poster.jpg",
                Events = events
            };
            Assert.Equal(id, movieData.Id);
            Assert.Equal("Test Movie", movieData.Title);
            Assert.Equal("Action", movieData.Genres);
            Assert.Equal("Actor 1, Actor 2", movieData.Actors);
            Assert.Equal("PG", movieData.AgeRating);
            Assert.Equal(120, movieData.Duration);
            Assert.Equal("http://example.com/poster.jpg", movieData.PosterUrl);
            Assert.Equal(events, movieData.Events);
        }

        [Fact]
        public void MovieData_DefaultValues_AreCorrect()
        {
            var movieData = new MovieData();
            Assert.Equal(Guid.Empty, movieData.Id);
            Assert.Null(movieData.Title);
            Assert.Null(movieData.Genres);
            Assert.Null(movieData.Actors);
            Assert.Null(movieData.AgeRating);
            Assert.Equal(0, movieData.Duration);
            Assert.Null(movieData.PosterUrl);
            Assert.NotNull(movieData.Events);
            Assert.Empty(movieData.Events);
        }
    }

    public class MovieEventDataTests
    {
        [Fact]
        public void MovieEventData_CanBeCreated_WithValidData()
        {
            var id = Guid.NewGuid();
            var date = DateOnly.FromDateTime(DateTime.UtcNow);
            var time = TimeOnly.FromDateTime(DateTime.UtcNow);
            var room = new RoomData { Id = Guid.NewGuid(), Name = "Room 1", Capacity = 50 };
            var eventData = new MovieEventData
            {
                Id = id,
                Date = date,
                Time = time,
                Room = room,
                Capacity = 100,
                Visitors = 10
            };
            Assert.Equal(id, eventData.Id);
            Assert.Equal(date, eventData.Date);
            Assert.Equal(time, eventData.Time);
            Assert.Equal(room, eventData.Room);
            Assert.Equal(100, eventData.Capacity);
            Assert.Equal(10, eventData.Visitors);
        }

        [Fact]
        public void MovieEventData_DefaultValues_AreCorrect()
        {
            var eventData = new MovieEventData();
            Assert.Equal(Guid.Empty, eventData.Id);
            Assert.Equal(default, eventData.Date);
            Assert.Equal(default, eventData.Time);
            Assert.Null(eventData.Room);
            Assert.Equal(0, eventData.Capacity);
            Assert.Equal(0, eventData.Visitors);
        }
    }
}
