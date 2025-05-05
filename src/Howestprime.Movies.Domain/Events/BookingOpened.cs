using System;
using Howestprime.Movies.Domain.Entities;

namespace Howestprime.Movies.Domain.Events
{
    public class BookingOpened
    {
        public Guid BookingId { get; }
        public Guid MovieEventId { get; }
        public int StandardVisitors { get; }
        public int DiscountVisitors { get; }
        public string RoomName { get; }
        public DateTime CreatedAt { get; }

        public BookingOpened(Booking booking, Guid movieEventId)
        {
            BookingId = booking.Id;
            MovieEventId = movieEventId;
            StandardVisitors = booking.StandardVisitors;
            DiscountVisitors = booking.DiscountVisitors;
            RoomName = booking.RoomName;
            CreatedAt = booking.CreatedAt;
        }
    }
}
