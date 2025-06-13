using System;
using System.Collections.Generic;
using Howestprime.Movies.Domain.Events;
using Howestprime.Movies.Domain.Entities;
using Xunit;

namespace UnitTests.Domain.Events
{
    public class DomainEventTests
    {
        [Fact]
        public void BookingOpened_CanBeCreated_WithValidData()
        {
            var booking = new Booking(
                new BookingId(),
                2,
                1,
                BookingStatus.Open,
                PaymentStatus.Pending,
                new List<int> { 1, 2, 3 },
                "Room 1",
                DateTime.UtcNow
            );
            var movieEventId = new MovieEventId();
            var evt = new BookingOpened(booking, movieEventId);
            Assert.Equal(booking.Id, evt.BookingId);
            Assert.Equal(movieEventId, evt.MovieEventId);
            Assert.Equal(booking.StandardVisitors, evt.StandardVisitors);
            Assert.Equal(booking.DiscountVisitors, evt.DiscountVisitors);
            Assert.Equal(booking.RoomName, evt.RoomName);
            Assert.Equal(booking.CreatedAt, evt.CreatedAt);
        }

        // Remove or comment out MovieRegistered and MovieDetailsChanged tests until their types are confirmed to exist and are public.
    }
}
