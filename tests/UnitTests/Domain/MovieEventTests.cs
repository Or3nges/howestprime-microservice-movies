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

        [Fact]
        public void BookEvent_Throws_When_Visitors_Negative()
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
            Assert.Throws<ArgumentException>(() => movieEvent.BookEvent(-1, 0, "Room 1"));
            Assert.Throws<ArgumentException>(() => movieEvent.BookEvent(0, -1, "Room 1"));
        }

        [Fact]
        public void BookEvent_Throws_When_Zero_Visitors()
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
            Assert.Throws<ArgumentException>(() => movieEvent.BookEvent(0, 0, "Room 1"));
        }

        [Fact]
        public void BookEvent_Throws_When_OverCapacity()
        {
            var movieEvent = new MovieEvent
            {
                Id = Guid.NewGuid(),
                MovieId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Time = TimeSpan.FromHours(15),
                Capacity = 5,
                Visitors = 4,
                Bookings = new List<Booking>()
            };
            Assert.Throws<InvalidOperationException>(() => movieEvent.BookEvent(2, 0, "Room 1"));
        }

        [Fact]
        public void BookEvent_Throws_When_BookingTooFarInAdvance()
        {
            var movieEvent = new MovieEvent
            {
                Id = Guid.NewGuid(),
                MovieId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Date = DateTime.UtcNow.AddDays(15),
                Time = TimeSpan.FromHours(15),
                Capacity = 100,
                Visitors = 0,
                Bookings = new List<Booking>()
            };
            Assert.Throws<InvalidOperationException>(() => movieEvent.BookEvent(1, 0, "Room 1"));
        }

        [Fact]
        public void MovieEvent_DefaultValues_AreCorrect()
        {
            var movieEvent = new MovieEvent();
            Assert.Equal(Guid.Empty, movieEvent.Id);
            Assert.Equal(Guid.Empty, movieEvent.MovieId);
            Assert.Equal(Guid.Empty, movieEvent.RoomId);
            Assert.Equal(default(DateTime), movieEvent.Date);
            Assert.Equal(default(TimeSpan), movieEvent.Time);
            Assert.Equal(0, movieEvent.Capacity);
            Assert.Equal(0, movieEvent.Visitors);
            Assert.NotNull(movieEvent.Bookings);
        }
    }
}
