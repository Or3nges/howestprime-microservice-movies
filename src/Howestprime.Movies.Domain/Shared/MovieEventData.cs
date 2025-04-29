namespace Howestprime.Movies.Domain.Shared
{
    public class MovieEventData
    {
        public Guid Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public MovieData Movie { get; set; }
        public RoomData Room { get; set; }
        public int Capacity { get; set; }
        public int Visitors { get; set; }
    }
}