using System;
using System.Collections.Generic;
using System.Linq;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Domain.MovieEvent;
using Howestprime.Movies.Domain.Room;
using Howestprime.Movies.Domain.Enums;
using Howestprime.Movies.Domain.Booking;
using Xunit;
using System.Threading.Tasks;

namespace UnitTests.Domain
{
    public class MovieEventTests
    {
        [Fact]
        public void Constructor_Succeeds()
        {
            var movieId = new MovieId();
            var roomId = new RoomId();
            var time = DateTime.UtcNow.AddDays(1);
            var capacity = 100;

            var movieEvent = new MovieEvent(new MovieEventId(), movieId, roomId, time, capacity);

            Assert.NotNull(movieEvent);
            Assert.Equal(movieId, movieEvent.MovieId);
            Assert.Equal(roomId, movieEvent.RoomId);
            Assert.Equal(time, movieEvent.Time);
            Assert.Equal(capacity, movieEvent.Capacity);
            Assert.Empty(movieEvent.Bookings);
        }

        [Fact]
        public void Constructor_WithPastTime_ThrowsException()
        {
            var movieId = new MovieId();
            var roomId = new RoomId();
            var time = DateTime.UtcNow.AddDays(-1);
            var capacity = 100;

            Assert.Throws<ArgumentException>(() => new MovieEvent(new MovieEventId(), movieId, roomId, time, capacity));
        }

        [Fact]
        public void Constructor_WithInvalidCapacity_ThrowsException()
        {
            var movieId = new MovieId();
            var roomId = new RoomId();
            var time = DateTime.UtcNow.AddDays(1);
            var capacity = 0;

            Assert.Throws<ArgumentException>(() => new MovieEvent(new MovieEventId(), movieId, roomId, time, capacity));
        }

        [Fact]
        public void BookEvent_Succeeds()
        {
            var movieEvent = new MovieEvent(new MovieEventId(), new MovieId(), new RoomId(), DateTime.UtcNow.AddDays(1), 10);
            var booking = movieEvent.BookEvent(2, 0, "Room A");
            
            Assert.NotNull(booking);
            Assert.Contains(booking, movieEvent.Bookings);
            Assert.Equal(2, booking.StandardVisitors);
            Assert.Equal(0, booking.DiscountVisitors);
            Assert.Equal("Room A", booking.RoomName);
            Assert.Equal(2, booking.SeatNumbers.Count);
        }

        [Fact]
        public void BookEvent_Fails_WhenNotEnoughSeats()
        {
            var movieEvent = new MovieEvent(new MovieEventId(), new MovieId(), new RoomId(), DateTime.UtcNow.AddDays(1), 1);
            Assert.Throws<InvalidOperationException>(() => movieEvent.BookEvent(2, 0, "Room A"));
        }

        [Fact]
        public void BookEvent_Fails_WhenInvalidVisitorCount()
        {
            var movieEvent = new MovieEvent(new MovieEventId(), new MovieId(), new RoomId(), DateTime.UtcNow.AddDays(1), 10);
            Assert.Throws<ArgumentException>(() => movieEvent.BookEvent(-1, 0, "Room A"));
        }

        [Fact]
        public void BookEvent_Fails_WhenInvalidDiscountVisitorCount()
        {
            var movieEvent = new MovieEvent(new MovieEventId(), new MovieId(), new RoomId(), DateTime.UtcNow.AddDays(1), 10);
            Assert.Throws<ArgumentException>(() => movieEvent.BookEvent(1, -1, "Room A"));
        }

        [Fact]
        public void BookEvent_Fails_WhenEmptyRoomName()
        {
            var movieEvent = new MovieEvent(new MovieEventId(), new MovieId(), new RoomId(), DateTime.UtcNow.AddDays(1), 10);
            Assert.Throws<ArgumentException>(() => movieEvent.BookEvent(1, 0, ""));
        }

        [Fact]
        public void BookEvent_Fails_WhenTooFarInFuture()
        {
            var movieEvent = new MovieEvent(new MovieEventId(), new MovieId(), new RoomId(), DateTime.UtcNow.AddDays(15), 10);
            Assert.Throws<InvalidOperationException>(() => movieEvent.BookEvent(1, 0, "Room A"));
        }

        [Fact]
        public void GetRemainingCapacity_ReturnsCorrectCount()
        {
            var movieEvent = new MovieEvent(new MovieEventId(), new MovieId(), new RoomId(), DateTime.UtcNow.AddDays(1), 10);
            movieEvent.BookEvent(2, 1, "Room A");
            movieEvent.BookEvent(3, 0, "Room B");

            var currentVisitors = movieEvent.Bookings.Sum(b => b.StandardVisitors + b.DiscountVisitors);
            Assert.Equal(4, movieEvent.Capacity - currentVisitors);
        }
    }
}
