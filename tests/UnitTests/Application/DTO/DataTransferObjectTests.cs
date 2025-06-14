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
    }
} 