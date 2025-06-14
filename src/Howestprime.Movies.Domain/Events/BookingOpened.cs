using System;
using System.Collections.Generic;
using Howestprime.Movies.Domain.Movie;

namespace Howestprime.Movies.Domain.Events
{
    public class BookingOpened
    {
        public string BookingId { get; }
        public string MovieId { get; }
        public string RoomName { get; }
        public DateTime Time { get; }
        public int StandardVisitors { get; }
        public int DiscountVisitors { get; }
        public List<int> SeatNumbers { get; }

        public BookingOpened(
            Domain.Booking.Booking booking,
            MovieId movieId,
            string roomName,
            DateTime time)
        {
            BookingId = booking.Id.Value;
            MovieId = movieId.Value;
            RoomName = roomName;
            Time = time;
            StandardVisitors = booking.StandardVisitors;
            DiscountVisitors = booking.DiscountVisitors;
            SeatNumbers = booking.SeatNumbers;
        }
    }
}
