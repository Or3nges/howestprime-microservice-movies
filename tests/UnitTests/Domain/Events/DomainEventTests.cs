using System;
using System.Collections.Generic;
using Howestprime.Movies.Domain.Events;
using Howestprime.Movies.Domain.Booking;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Domain.MovieEvent;
using Howestprime.Movies.Domain.Enums;
using Xunit;

namespace UnitTests.Domain.Events
{
    public class DomainEventTests
    {
        [Fact]
        public void BookingOpenedEvent_HasCorrectData()
        {
            var bookingId = new BookingId();
            var movieId = new MovieId();
            var roomName = "Room Alpha";
            var time = DateTime.UtcNow;
            var seatNumbers = new List<int> { 5, 6 };

            var booking = new Booking(bookingId, new MovieEventId(), 1, 1, Howestprime.Movies.Domain.Booking.BookingStatus.Open, PaymentStatus.Pending, seatNumbers, roomName, time);
            var bookingOpenedEvent = new BookingOpened(booking, movieId, roomName, time);

            Assert.Equal(booking.Id.Value, bookingOpenedEvent.BookingId);
            Assert.Equal(movieId.Value, bookingOpenedEvent.MovieId);
            Assert.Equal(roomName, bookingOpenedEvent.RoomName);
            Assert.Equal(seatNumbers, bookingOpenedEvent.SeatNumbers);
            Assert.Equal(time, bookingOpenedEvent.Time);
        }

        // Remove or comment out MovieRegistered and MovieDetailsChanged tests until their types are confirmed to exist and are public.
    }
}
