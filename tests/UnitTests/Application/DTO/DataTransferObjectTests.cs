using System;
using Xunit;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Movies.FindMovieByIdWithEvents;
using Howestprime.Movies.Application.Movies.ScheduleMovieEvent;
using Howestprime.Movies.Domain.MovieEvent;
using Howestprime.Movies.Domain.Movie;

namespace UnitTests.Application.DTO
{
    public class DataTransferObjectTests
    {
        [Fact]
        public void ExtendedMovieEventData_CanBeCreated()
        {
            var item = new ExtendedMovieEventData
            {
                Id = Guid.NewGuid(),
                Room = new RoomData(),
                Time = DateTime.UtcNow,
                Movie = new MovieData(),
                Capacity = 100
            };

            Assert.NotEqual(Guid.Empty, item.Id);
            Assert.NotNull(item.Room);
            Assert.NotNull(item.Movie);
            Assert.True(item.Time > DateTime.MinValue);
            Assert.Equal(100, item.Capacity);
        }

        [Fact]
        public void MovieEventResultData_CanBeCreated()
        {
            var item = new MovieEventResultData
            {
                Id = new MovieEventId(),
                Time = DateTime.UtcNow,
                Capacity = 100,
                Room = new RoomResultData()
            };

            Assert.NotNull(item.Id);
            Assert.NotNull(item.Room);
            Assert.True(item.Time > DateTime.MinValue);
            Assert.Equal(100, item.Capacity);
        }

        [Fact]
        public void RoomResultData_CanBeCreated()
        {
            var item = new RoomResultData
            {
                Id = "Room1",
                Name = "Room 1",
                Capacity = 150
            };

            Assert.Equal("Room1", item.Id);
            Assert.Equal("Room 1", item.Name);
            Assert.Equal(150, item.Capacity);
        }
        
        [Fact]
        public void ScheduleMovieEventResult_CanBeCreated()
        {
            var movieEventId = new MovieEventId();
            var item = new ScheduleMovieEventResult
            {
                MovieEventId = movieEventId
            };
            
            Assert.Equal(movieEventId, item.MovieEventId);
        }

        [Fact]
        public void ScheduledMovieEventsResponse_CanBeCreated()
        {
            var item = new Howestprime.Movies.Application.Movies.FindMovieEventsForMonth.ScheduledMovieEventsResponse
            {
                Data = new System.Collections.Generic.List<ExtendedMovieEventData>()
            };

            Assert.NotNull(item.Data);
        }

        [Fact]
        public void FindMovieByIdResult_CanBeCreated()
        {
            var item = new Howestprime.Movies.Application.UseCases.Movies.FindMovieById.FindMovieByIdResult
            {
                Id = new MovieId(),
                Title = "Test Movie",
                Year = 2024
            };

            Assert.NotNull(item.Id);
            Assert.Equal("Test Movie", item.Title);
            Assert.Equal(2024, item.Year);
        }

        [Fact]
        public void FindMovieByIdWithEventsResult_CanBeCreated()
        {
            var item = new Howestprime.Movies.Application.UseCases.Movies.FindMovieByIdWithEvents.FindMovieByIdWithEventsResult
            {
                Id = new MovieId(),
                Title = "Test Movie",
                Year = 2024,
                Events = new System.Collections.Generic.List<MovieEventResultData>()
            };

            Assert.NotNull(item.Id);
            Assert.Equal("Test Movie", item.Title);
            Assert.Equal(2024, item.Year);
            Assert.NotNull(item.Events);
        }
    }
} 