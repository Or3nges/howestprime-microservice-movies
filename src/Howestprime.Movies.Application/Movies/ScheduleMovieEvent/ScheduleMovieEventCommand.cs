using System;

namespace Howestprime.Movies.Application.Movies.ScheduleMovieEvent
{
    public class ScheduleMovieEventCommand
    {
        public Guid MovieId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public int Capacity { get; set; }
        public int Visitors { get; set; }
    }
}
