using System;
using System.Collections.Generic;
using System.Linq;
using Howestprime.Movies.Domain.Enums;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Domain.Room;
using Domaincrafters.Domain;

namespace Howestprime.Movies.Domain.MovieEvent
{
    public sealed class MovieEventId : UuidEntityId
    {
        public MovieEventId(string? id = "") : base(id)
        {
        }

        public static MovieEventId CreateUnique()
        {
            return new MovieEventId(Guid.NewGuid().ToString());
        }
    }

    public class MovieEvent : Entity<MovieEventId>
    {
        public MovieId MovieId { get; private set; }
        public RoomId RoomId { get; private set; }
        public DateTime Time { get; private set; }
        public int Capacity { get; private set; }
        public List<Domain.Booking.Booking> Bookings { get; private set; } = new List<Domain.Booking.Booking>();

        public MovieEvent(MovieEventId id, MovieId movieId, RoomId roomId, DateTime time, int capacity)
            : base(id)
        {
            if (capacity <= 0)
                throw new ArgumentException("Capacity must be greater than 0.");

            if (time < DateTime.UtcNow)
                throw new ArgumentException("Movie event cannot be scheduled in the past.");

            MovieId = movieId;
            RoomId = roomId;
            Time = time;
            Capacity = capacity;
            ValidateState();
        }
        
        public Domain.Booking.Booking BookEvent(int standardVisitors, int discountVisitors, string roomName)
        {
            if (standardVisitors < 0 || discountVisitors < 0)
                throw new ArgumentException("Visitor counts must be non-negative.");
            int totalVisitors = standardVisitors + discountVisitors;
            if (totalVisitors <= 0)
                throw new ArgumentException("Total visitor count must be greater than 0.");

            var currentVisitors = Bookings.Sum(b => b.StandardVisitors + b.DiscountVisitors);
            if (currentVisitors + totalVisitors > Capacity)
                throw new InvalidOperationException("Cannot book more visitors than room capacity.");
                
            if ((Time - DateTime.UtcNow).TotalDays > 14)
                throw new InvalidOperationException("Movie event can only be booked within 14 days.");

            if (string.IsNullOrWhiteSpace(roomName))
                throw new ArgumentException("Room name cannot be null or empty when booking a movie event.");

            var seatNumbers = new List<int>();
            for (int i = 1; i <= totalVisitors; i++)
                seatNumbers.Add(currentVisitors + i);

            var booking = new Domain.Booking.Booking(
                new Domain.Booking.BookingId(),
                Id,
                discountVisitors,
                standardVisitors,
                Domain.Booking.BookingStatus.Open,
                PaymentStatus.Pending,
                seatNumbers,
                roomName,
                DateTime.UtcNow
            );
            Bookings.Add(booking);

            return booking;
        }

        public override void ValidateState()
        {
            if (Capacity <= 0)
                throw new InvalidOperationException("Capacity must be greater than 0.");
        }
    }
}
