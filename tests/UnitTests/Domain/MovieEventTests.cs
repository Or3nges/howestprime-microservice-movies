using System;
using System.Collections.Generic;
using Howestprime.Movies.Domain.Entities;
using Xunit;

namespace UnitTests.Domain
{
    public class MovieEventTests
    {
        [Fact]
        public void MovieEvent_CanBeCreated_WithValidData()
        {
            var id = Guid.NewGuid();
            var movieId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var time = TimeSpan.FromHours(15);
            var movieEvent = new MovieEvent
            {
                Id = id,
                MovieId = movieId,
                RoomId = roomId,
                Date = date,
                Time = time,
                Capacity = 100,
                Visitors = 0,
                Bookings = new List<Booking>()
            };
            Assert.Equal(id, movieEvent.Id);
            Assert.Equal(movieId, movieEvent.MovieId);
            Assert.Equal(roomId, movieEvent.RoomId);
            Assert.Equal(date, movieEvent.Date);
            Assert.Equal(time, movieEvent.Time);
            Assert.Equal(100, movieEvent.Capacity);
            Assert.Equal(0, movieEvent.Visitors);
            Assert.Empty(movieEvent.Bookings);
        }

        [Fact]
        public void MovieEvent_BookEvent_AddsBookingAndIncrementsVisitors()
        {
            var movieEvent = new MovieEvent
            {
                Id = Guid.NewGuid(),
                MovieId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Time = TimeSpan.FromHours(15),
                Capacity = 100,
                Visitors = 0,
                Bookings = new List<Booking>()
            };
            int initialVisitors = movieEvent.Visitors;
            int standard = 2, discount = 1;
            var booking = movieEvent.BookEvent(standard, discount, "Room 1");
            Assert.Contains(booking, movieEvent.Bookings);
            Assert.Equal(initialVisitors + standard + discount, movieEvent.Visitors);
        }
    }
}
