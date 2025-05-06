using System;
using System.Collections.Generic;
using Howestprime.Movies.Domain.Shared;
using Xunit;

namespace UnitTests.Domain.Shared
{
    public class MovieEventTests
    {
        [Fact]
        public void MovieEvent_CanBeCreated_WithValidData()
        {
            var id = Guid.NewGuid();
            var movieId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var date = DateOnly.FromDateTime(DateTime.Today);
            var time = TimeOnly.FromTimeSpan(TimeSpan.FromHours(15));
            var visitors = 5;
            var bookings = new List<Guid> { Guid.NewGuid() };
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
            var movieEvent = new MovieEvent(Guid.Empty, Guid.Empty, Guid.Empty, default, default, 0, new List<Guid>());
            Assert.Equal(Guid.Empty, movieEvent.Id);
            Assert.Equal(Guid.Empty, movieEvent.MovieId);
            Assert.Equal(Guid.Empty, movieEvent.RoomId);
            Assert.Equal(default(DateOnly), movieEvent.Date);
            Assert.Equal(default(TimeOnly), movieEvent.Time);
            Assert.Equal(0, movieEvent.Visitors);
            Assert.Empty(movieEvent.Bookings);
        }
    }
}
