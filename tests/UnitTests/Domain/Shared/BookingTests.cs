using System;
using Howestprime.Movies.Domain.Shared;
using Howestprime.Movies.Domain.Enums;
using Xunit;

namespace UnitTests.Domain.Shared
{
    public class BookingTests
    {
        [Fact]
        public void Booking_CanBeCreated_WithValidData()
        {
            var id = Guid.NewGuid();
            var movieEventId = Guid.NewGuid();
            var visitors = 5;
            var discountedVisitors = 2;
            var standardVisitors = 3;
            var status = Howestprime.Movies.Domain.Shared.BookingStatus.Open;
            var booking = new Booking(id, movieEventId, visitors, discountedVisitors, standardVisitors, status);
            Assert.Equal(id, booking.Id);
            Assert.Equal(movieEventId, booking.MovieEventId);
            Assert.Equal(visitors, booking.Visitors);
            Assert.Equal(discountedVisitors, booking.DiscountedVisitors);
            Assert.Equal(standardVisitors, booking.StandardVisitors);
            Assert.Equal(status, booking.Status);
        }
    }
}
