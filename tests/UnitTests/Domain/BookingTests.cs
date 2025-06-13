using System;
using System.Collections.Generic;
using Howestprime.Movies.Domain.Entities;
using Howestprime.Movies.Domain.Enums;
using Xunit;

namespace UnitTests.Domain
{
    public class BookingTests
    {
        [Fact]
        public void CreateBooking_Succeeds_WithValidData()
        {
            var booking = new Booking(new BookingId(), 1, 1, BookingStatus.Open, PaymentStatus.Pending,
                new List<int> { 1, 2 }, "Room A", DateTime.UtcNow);

            booking.ValidateState();

            Assert.NotNull(booking);
            Assert.Equal(1, booking.StandardVisitors);
            Assert.Equal(1, booking.DiscountVisitors);
            Assert.Equal(BookingStatus.Open, booking.Status);
            Assert.Equal(PaymentStatus.Pending, booking.PaymentStatus);
        }
        
        [Fact]
        public void ValidateState_ThrowsException_WhenStandardVisitorsIsNegative()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var booking = new Booking(new BookingId(), -1, 1, BookingStatus.Open, PaymentStatus.Pending,
                    new List<int> { 1, 2 }, "Room A", DateTime.UtcNow);
                booking.ValidateState();
            });
        }
        
        [Fact]
        public void ValidateState_ThrowsException_WhenRoomNameIsEmpty()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var booking = new Booking(new BookingId(), 1, 1, BookingStatus.Open, PaymentStatus.Pending,
                    new List<int> { 1, 2 }, "", DateTime.UtcNow);
                booking.ValidateState();
            });
        }
        
        [Fact]
        public void ValidateState_ThrowsException_WhenSeatNumbersAreEmpty()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var booking = new Booking(new BookingId(), 1, 1, BookingStatus.Open, PaymentStatus.Pending,
                    new List<int>(), "Room A", DateTime.UtcNow);
                booking.ValidateState();
            });
        }
    }
} 