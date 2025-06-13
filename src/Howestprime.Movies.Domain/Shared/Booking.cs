using Howestprime.Movies.Domain.Entities;

namespace Howestprime.Movies.Domain.Shared
{
    public enum BookingStatus
    {
        Open,
        Closed
    }

    public class Booking
    {
        public BookingId Id { get; private set; }
        public MovieEventId MovieEventId { get; private set; }
        public int Visitors { get; private set; }
        public int DiscountedVisitors { get; private set; }
        public int StandardVisitors { get; private set; }
        public BookingStatus Status { get; private set; }

        public Booking(BookingId id, MovieEventId movieEventId, int visitors, int discountedVisitors, int standardVisitors, BookingStatus status)
        {
            Id = id;
            MovieEventId = movieEventId;
            Visitors = visitors;
            DiscountedVisitors = discountedVisitors;
            StandardVisitors = standardVisitors;
            Status = status;
        }
    }
}