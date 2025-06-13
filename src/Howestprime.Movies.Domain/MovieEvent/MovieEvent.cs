using System;
using System.Collections.Generic;
using System.Linq;
using Domaincrafters.Domain;
using Howestprime.Movies.Domain.Enums;
using Howestprime.Movies.Domain.Shared;
using Howestprime.Movies.Domain.Entities;
using Howestprime.Movies.Domain.Movie; // Added for MovieId
// Assuming a Booking entity and its related types will be refactored similarly.
// For now, using existing Booking structure for BookEvent method.

namespace Howestprime.Movies.Domain.MovieEvent
{
    public class MovieEvent : Entity<MovieEventId>
    {
        public MovieId MovieId { get; private set; }
        public RoomId RoomId { get; private set; }
        public DateTime Time { get; private set; }
        public int Capacity { get; private set; }
        public int Visitors { get; private set; } // Made private set
        // public List<BookingId> BookingIds { get; private set; } = new List<BookingId>(); // Keep track of associated bookings

        // Add these properties if they don't exist
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int AvailableSeats { get; set; }

        private MovieEvent(MovieEventId id, MovieId movieId, RoomId roomId, DateTime time, int capacity) : base(id)
        {
            MovieId = movieId;
            RoomId = roomId;
            Time = time;
            Capacity = capacity;
            Visitors = 0; // Initial visitors count
        }

        public MovieEvent(MovieId movieId, RoomId roomId, DateTime startTime, DateTime endTime, int capacity)
        : base(MovieEventId.CreateUnique())
        {
            MovieId = movieId;
            RoomId = roomId;
            StartTime = startTime;
            EndTime = endTime;
            Capacity = capacity;
            Visitors = 0;
        }

        public static MovieEvent Create(MovieId movieId, RoomId roomId, DateTime time, int capacity)
        {
            var movieEventId = MovieEventId.CreateUnique();
            var movieEvent = new MovieEvent(movieEventId, movieId, roomId, time, capacity);
            movieEvent.ValidateState();
            // Consider publishing a MovieEventScheduled event here if needed
            // DomainEventPublisher.Instance.Publish(MovieEventScheduled.Create(movieEvent.Id, movieEvent.MovieId, movieEvent.Time));
            return movieEvent;
        }

        // Renamed and refactored from BookEvent
        public List<int> ReserveSeats(int standardVisitors, int discountVisitors)
        {
            EnsureCanReserve(standardVisitors, discountVisitors);

            int totalNewVisitors = standardVisitors + discountVisitors;
            var allocatedSeatNumbers = new List<int>();
            for (int i = 1; i <= totalNewVisitors; i++)
            {
                allocatedSeatNumbers.Add(Visitors + i); // Simple seat allocation logic
            }

            Visitors += totalNewVisitors;
            // No longer creates Booking entity here
            // No longer adds to a Bookings list here, as Booking is its own aggregate.
            // We might add BookingId to BookingIds list if needed for querying,
            // but the primary relationship is Booking -> MovieEventId.

            // ValidateState(); // Ensure state is still valid after change
            return allocatedSeatNumbers;
        }

        private void EnsureCanReserve(int standardVisitors, int discountVisitors)
        {
            if (standardVisitors < 0 || discountVisitors < 0)
                throw new ArgumentException("Visitor counts must be non-negative.");

            int totalNewVisitors = standardVisitors + discountVisitors;
            if (totalNewVisitors <= 0)
                throw new ArgumentException("Total visitor count must be greater than 0.");

            if (Visitors + totalNewVisitors > Capacity)
                throw new InvalidOperationException("Cannot book more visitors than room capacity.");
            
            // Ensure booking is within the allowed window (e.g., 14 days from event time)
            // This check was present in the old BookEvent method.
            // Assuming 'Time' is the event's start time.
            if ((Time - DateTime.UtcNow).TotalDays > 14) // Or from a configurable setting
                 throw new InvalidOperationException("Movie event can only be booked up to 14 days in advance.");

            // Add any other pre-condition checks for reserving seats.
        }
        
        // Add this method to be called by the handler after a booking is successfully created and saved.
        // This is an alternative to directly modifying Visitors in ReserveSeats if we want to separate
        // the "attempt" from the "confirmation". However, for simplicity, ReserveSeats now updates Visitors.
        // public void ConfirmBookingReservation(int totalBookedVisitors)
        // {
        //     Visitors += totalBookedVisitors;
        //     ValidateState();
        // }


        public override void ValidateState()
        {
            EnsureTimeIsValid(Time);
            EnsureCapacityIsValid(Capacity);
            // base.ValidateState(); // Removed: Cannot call an abstract base member
            if (Visitors < 0)
                throw new ArgumentOutOfRangeException(nameof(Visitors), "Visitor count cannot be negative.");
            if (Visitors > Capacity)
                throw new InvalidOperationException("Visitor count cannot exceed capacity.");
        }

        private static void EnsureTimeIsValid(DateTime time)
        {
            if (time == default)
            {
                throw new ArgumentException("Time must be a valid date and time.", nameof(time));
            }
            // Add other relevant time validations, e.g., not in the past for new events.
        }

        private static void EnsureCapacityIsValid(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
            }
            // Max capacity could be a system-wide setting or room-specific.
        }
    }
}
