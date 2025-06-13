using System;
using Howestprime.Movies.Domain.Entities;
using Howestprime.Movies.Domain.MovieEvent; 

namespace Howestprime.Movies.Application.Movies.FindMovieByIdWithEvents
{
    public class MovieEventResultData
    {
        public MovieEventId Id { get; set; }
        public DateTime Time { get; set; }
        public int Capacity { get; set; }
        public RoomResultData Room { get; set; }
    }

    public class RoomResultData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
    }
}
