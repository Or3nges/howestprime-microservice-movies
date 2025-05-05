using System;
using Howestprime.Movies.Domain.Entities;
using Howestprime.Movies.Domain.Enums;
using Xunit;

namespace UnitTests.Domain
{
    public class BookingTests
    {
        [Fact]
        public void Booking_CanBeCreated_WithValidData()
        {
            // Arrange
            var id = Guid.NewGuid();
            var roomName = "Room A";
            var createdAt = DateTime.UtcNow;

            // Act
            var booking = new Booking
            {
                Id = id,
                StandardVisitors = 2,
                DiscountVisitors = 1,
                Status = BookingStatus.Open,
                PaymentStatus = PaymentStatus.Pending,
                RoomName = roomName,
                CreatedAt = createdAt
            };

            // Assert
            Assert.Equal(id, booking.Id);
            Assert.Equal(2, booking.StandardVisitors);
            Assert.Equal(1, booking.DiscountVisitors);
            Assert.Equal(BookingStatus.Open, booking.Status);
            Assert.Equal(PaymentStatus.Pending, booking.PaymentStatus);
            Assert.Equal(roomName, booking.RoomName);
            Assert.Equal(createdAt, booking.CreatedAt);
        }
    }
}
