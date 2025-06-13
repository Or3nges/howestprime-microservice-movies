using Howestprime.Movies.Domain.Entities;

namespace Howestprime.Movies.Application.Movies.BookMovieEvent
{
    public class BookMovieEventCommand
    {
        public MovieEventId MovieEventId { get; set; }
        public int StandardVisitors { get; set; }
        public int DiscountVisitors { get; set; }
        public string? RoomName { get; set; } // Make nullable to fix warning
    }
}