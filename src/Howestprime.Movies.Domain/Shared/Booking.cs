namespace Howestprime.Movies.Domain.Shared
{
    public enum BookingStatus
    {
        Open,
        Closed
    }

    public class Booking
    {
        public Guid Id { get; private set; }
        public Guid MovieEventId { get; private set; }
        public int Visitors { get; private set; }
        public int DiscountedVisitors { get; private set; }
        public int StandardVisitors { get; private set; }
        public BookingStatus Status { get; private set; }

        public Booking(Guid id, Guid movieEventId, int visitors, int discountedVisitors, int standardVisitors, BookingStatus status)
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