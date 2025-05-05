using System;
using System.Collections.Generic;
using Howestprime.Movies.Domain.Enums;

namespace Howestprime.Movies.Domain.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }
        public int StandardVisitors { get; set; }
        public int DiscountVisitors { get; set; }
        public BookingStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public List<int> SeatNumbers { get; set; } = new List<int>();
        public string RoomName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
