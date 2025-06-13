using System;
using System.Collections.Generic;
using Howestprime.Movies.Domain.Shared;
using Howestprime.Movies.Domain.Entities;
using Xunit;

namespace UnitTests.Domain.Shared
{
    public class MovieEventTests
    {
        [Fact]
        public void MovieEvent_CanBeCreated_WithValidData()
        {
            var id = new MovieEventId();
            var movieId = new MovieId();
            var roomId = new RoomId();
            var date = DateOnly.FromDateTime(DateTime.Today);
            var time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(15));
            var visitors = 5;
            var bookings = new List<BookingId> { new BookingId() };
            var movieEvent = new MovieEvent(id, movieId, roomId, date, time, visitors, bookings);
            Assert.Equal(id, movieEvent.Id);
            Assert.Equal(movieId, movieEvent.MovieId);
            Assert.Equal(roomId, movieEvent.RoomId);
            Assert.Equal(date, movieEvent.Date);
            Assert.Equal(time, movieEvent.Time);
            Assert.Equal(visitors, movieEvent.Visitors);
            Assert.Equal(bookings, movieEvent.Bookings);
        }

        [Fact]
        public void MovieEvent_DefaultValues_AreCorrect()
        {
            var movieEvent = new MovieEvent(
                new MovieEventId(),
                new MovieId(),
                new RoomId(),
                default,
                default,
                0,
                new List<BookingId>()
            );
            Assert.Equal(new MovieEventId(), movieEvent.Id);
            Assert.Equal(new MovieId(), movieEvent.MovieId);
            Assert.Equal(new RoomId(), movieEvent.RoomId);
            Assert.Equal(default(DateOnly), movieEvent.Date);
            Assert.Equal(default(TimeOnly), movieEvent.Time);
            Assert.Equal(0, movieEvent.Visitors);
            Assert.Empty(movieEvent.Bookings);
        }
    }
}
