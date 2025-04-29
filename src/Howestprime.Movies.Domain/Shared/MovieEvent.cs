namespace Howestprime.Movies.Domain.Shared
{
    public class MovieEvent
    {
        public Guid Id { get; private set; }
        public Guid MovieId { get; private set; }
        public Guid RoomId { get; private set; }
        public DateOnly Date { get; private set; }
        public TimeOnly Time { get; private set; }
        public int Visitors { get; private set; }
        public List<Guid> Bookings { get; private set; }

        public MovieEvent(Guid id, Guid movieId, Guid roomId, DateOnly date, TimeOnly time, int visitors, List<Guid> bookings)
        {
            Id = id;
            MovieId = movieId;
            RoomId = roomId;
            Date = date;
            Time = time;
            Visitors = visitors;
            Bookings = bookings ?? new List<Guid>();
        }
    }
}