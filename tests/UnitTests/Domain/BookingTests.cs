using System;
using System.Collections.Generic;
using Howestprime.Movies.Domain.Booking;
using Howestprime.Movies.Domain.MovieEvent;
using Howestprime.Movies.Domain.Enums;
using Xunit;

namespace UnitTests.Domain
{
    public class BookingTests
    {
        [Fact]
        public void CreateBooking_Succeeds_WithValidData()
        {
            var createdAt = DateTime.UtcNow;
            var booking = new Booking(new BookingId(), new MovieEventId(), 1, 1, Howestprime.Movies.Domain.Booking.BookingStatus.Open, PaymentStatus.Pending,
                new List<int> { 1, 2 }, "Room A", createdAt);

            booking.ValidateState();

            Assert.NotNull(booking);
            Assert.Equal(1, booking.StandardVisitors);
            Assert.Equal(1, booking.DiscountVisitors);
            Assert.Equal(Howestprime.Movies.Domain.Booking.BookingStatus.Open, booking.Status);
            Assert.Equal(PaymentStatus.Pending, booking.PaymentStatus);
        }
        
        [Fact]
        public void ValidateState_ThrowsException_WhenStandardVisitorsIsNegative()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var createdAt = DateTime.UtcNow;
                var booking = new Booking(new BookingId(), new MovieEventId(), -1, 1, Howestprime.Movies.Domain.Booking.BookingStatus.Open, PaymentStatus.Pending,
                    new List<int> { 1, 2 }, "Room A", createdAt);
                booking.ValidateState();
            });
        }
        
        [Fact]
        public void ValidateState_ThrowsException_WhenRoomNameIsEmpty()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var createdAt = DateTime.UtcNow;
                var booking = new Booking(new BookingId(), new MovieEventId(), 1, 1, Howestprime.Movies.Domain.Booking.BookingStatus.Open, PaymentStatus.Pending,
                    new List<int> { 1, 2 }, "", createdAt);
                booking.ValidateState();
            });
        }
        
        [Fact]
        public void ValidateState_ThrowsException_WhenSeatNumbersAreEmpty()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var createdAt = DateTime.UtcNow;
                var booking = new Booking(new BookingId(), new MovieEventId(), 1, 1, Howestprime.Movies.Domain.Booking.BookingStatus.Open, PaymentStatus.Pending,
                    new List<int>(), "Room A", createdAt);
                booking.ValidateState();
            });
        }
    }
} 