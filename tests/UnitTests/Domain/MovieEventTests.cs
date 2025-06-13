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
            var id = new MovieEventId();
            var movieId = new MovieId();
            var roomId = new RoomId();
            var time = DateTime.UtcNow.Add(TimeSpan.FromHours(15));
            var movieEvent = new MovieEvent(id, movieId, roomId, time, 100);
            Assert.Equal(id, movieEvent.Id);
            Assert.Equal(movieId, movieEvent.MovieId);
            Assert.Equal(roomId, movieEvent.RoomId);
            Assert.Equal(time, movieEvent.Time);
            Assert.Equal(100, movieEvent.Capacity);
            Assert.Equal(0, movieEvent.Visitors);
            Assert.Empty(movieEvent.Bookings);
        }

        [Fact]
        public void MovieEvent_BookEvent_AddsBookingAndIncrementsVisitors()
        {
            var movieEvent = new MovieEvent(
                new MovieEventId(),
                new MovieId(),
                new RoomId(),
                DateTime.UtcNow.Add(TimeSpan.FromHours(15)),
                100
            );
            int initialVisitors = movieEvent.Visitors;
            int standard = 2, discount = 1;
            var booking = movieEvent.BookEvent(standard, discount, "Room 1");
            Assert.Contains(booking, movieEvent.Bookings);
            Assert.Equal(initialVisitors + standard + discount, movieEvent.Visitors);
        }

        [Fact]
        public void BookEvent_Throws_When_Visitors_Negative()
        {
            var movieEvent = new MovieEvent(
                new MovieEventId(),
                new MovieId(),
                new RoomId(),
                DateTime.UtcNow.Add(TimeSpan.FromHours(15)),
                100
            );
            Assert.Throws<ArgumentException>(() => movieEvent.BookEvent(-1, 0, "Room 1"));
            Assert.Throws<ArgumentException>(() => movieEvent.BookEvent(0, -1, "Room 1"));
        }

        [Fact]
        public void BookEvent_Throws_When_TotalVisitors_Zero()
        {
            var movieEvent = new MovieEvent(
                new MovieEventId(),
                new MovieId(),
                new RoomId(),
                DateTime.UtcNow.Add(TimeSpan.FromHours(15)),
                100
            );
            Assert.Throws<ArgumentException>(() => movieEvent.BookEvent(0, 0, "Room 1"));
        }

        [Fact]
        public void BookEvent_Throws_When_ExceedsCapacity()
        {
            var movieEvent = new MovieEvent(
                new MovieEventId(),
                new MovieId(),
                new RoomId(),
                DateTime.UtcNow.Add(TimeSpan.FromHours(15)),
                100
            );
            movieEvent.BookEvent(50, 50, "Room 1"); // Fill up the room
            Assert.Throws<InvalidOperationException>(() => movieEvent.BookEvent(1, 0, "Room 1"));
        }

        [Fact]
        public void BookEvent_Throws_When_TooFarInFuture()
        {
            var movieEvent = new MovieEvent(
                new MovieEventId(),
                new MovieId(),
                new RoomId(),
                DateTime.UtcNow.AddDays(15), // More than 14 days in the future
                100
            );
            Assert.Throws<InvalidOperationException>(() => movieEvent.BookEvent(1, 0, "Room 1"));
        }

        [Fact]
        public void BookEvent_Throws_When_RoomNameIsNullOrEmpty()
        {
            var movieEvent = new MovieEvent(
                new MovieEventId(),
                new MovieId(),
                new RoomId(),
                DateTime.UtcNow.Add(TimeSpan.FromHours(15)),
                100
            );
            
            var nullException = Assert.Throws<ArgumentException>(
                () => movieEvent.BookEvent(2, 1, null)
            );
            Assert.Equal("Room name cannot be null or empty when booking a movie event.", nullException.Message);
            
            var emptyException = Assert.Throws<ArgumentException>(
                () => movieEvent.BookEvent(2, 1, "")
            );
            Assert.Equal("Room name cannot be null or empty when booking a movie event.", emptyException.Message);
            
            var whitespaceException = Assert.Throws<ArgumentException>(
                () => movieEvent.BookEvent(2, 1, "   ")
            );
            Assert.Equal("Room name cannot be null or empty when booking a movie event.", whitespaceException.Message);
        }
    }
}
