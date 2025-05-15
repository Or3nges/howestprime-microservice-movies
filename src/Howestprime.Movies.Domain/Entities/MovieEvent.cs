using System;
using System.Collections.Generic;
using Howestprime.Movies.Domain.Enums;

namespace Howestprime.Movies.Domain.Entities
{
    public class MovieEvent
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int Capacity { get; set; }
        public int Visitors { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
                public Booking BookEvent(int standardVisitors, int discountVisitors, string roomName)
        {
            if (standardVisitors < 0 || discountVisitors < 0)
                throw new ArgumentException("Visitor counts must be non-negative.");
            int totalVisitors = standardVisitors + discountVisitors;
            if (totalVisitors <= 0)
                throw new ArgumentException("Total visitor count must be greater than 0.");
            if (Visitors + totalVisitors > Capacity)
                throw new InvalidOperationException("Cannot book more visitors than room capacity.");
            if ((Date - DateTime.UtcNow).TotalDays > 14)
                throw new InvalidOperationException("Movie event can only be booked within 14 days.");

            var seatNumbers = new List<int>();
            for (int i = 1; i <= totalVisitors; i++)
                seatNumbers.Add(Visitors + i);
                            if (string.IsNullOrWhiteSpace(roomName))
            {
                throw new ArgumentException("Room name cannot be null or empty when booking a movie event.");
            }

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                StandardVisitors = standardVisitors,
                DiscountVisitors = discountVisitors,
                Status = BookingStatus.Open,
                PaymentStatus = PaymentStatus.Pending,
                SeatNumbers = seatNumbers,
                RoomName = roomName,
                CreatedAt = DateTime.UtcNow
            };
            Bookings.Add(booking);
            Visitors += totalVisitors;

            return booking;
        }
    }
}
