namespace Howestprime.Movies.Application.Movies.BookMovieEvent
{
    public class BookMovieEventCommand
    {
        public Guid MovieEventId { get; set; }
        public int StandardVisitors { get; set; }
        public int DiscountVisitors { get; set; }
        public string RoomName { get; set; }
    }
}