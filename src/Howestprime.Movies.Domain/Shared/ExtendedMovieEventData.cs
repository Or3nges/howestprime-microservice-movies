using System;

namespace Howestprime.Movies.Domain.Shared
{
    public class ExtendedMovieEventData
    {
        public Guid Id { get; set; }
        public RoomData Room { get; set; }
        public DateTime Time { get; set; }  
        public MovieData Movie { get; set; }
        public int Capacity { get; set; }
    }
}