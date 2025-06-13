using System;
using System.Collections.Generic;
using Howestprime.Movies.Domain.Entities;
using Howestprime.Movies.Domain.Enums;
using Xunit;

namespace UnitTests.Domain
{
    public class MovieEventTests
    {
        [Fact]
        public void CreateMovieEvent_Succeeds()
        {
            var movieId = new MovieId();
            var roomId = new RoomId();
            var movieEvent = new MovieEvent(new MovieEventId(), movieId, roomId, DateTime.UtcNow, 100);

            Assert.NotNull(movieEvent);
            Assert.Equal(movieId, movieEvent.MovieId);
            Assert.Equal(roomId, movieEvent.RoomId);
            Assert.Equal(100, movieEvent.Capacity);
        }

        [Fact]
        public void BookEvent_Succeeds()
        {
            var movieEvent = new MovieEvent(new MovieEventId(), new MovieId(), new RoomId(), DateTime.UtcNow, 10);
            var booking = movieEvent.BookEvent(2, 0, "Room A");
            
            Assert.NotNull(booking);
            Assert.Contains(booking, movieEvent.Bookings);
        }

        [Fact]
        public void BookEvent_Fails_WhenNotEnoughSeats()
        {
            var movieEvent = new MovieEvent(new MovieEventId(), new MovieId(), new RoomId(), DateTime.UtcNow, 1);
            Assert.Throws<InvalidOperationException>(() => movieEvent.BookEvent(2, 0, "Room A"));
        }

        [Fact]
        public void BookEvent_Fails_WhenTooFarInFuture()
        {
            var movieEvent = new MovieEvent(new MovieEventId(), new MovieId(), new RoomId(), DateTime.UtcNow.AddDays(20), 10);
            Assert.Throws<InvalidOperationException>(() => movieEvent.BookEvent(1, 0, "Room A"));
        }
    }
}
