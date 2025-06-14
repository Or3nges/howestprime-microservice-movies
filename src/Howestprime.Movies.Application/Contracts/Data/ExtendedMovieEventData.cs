using System;
using Howestprime.Movies.Application.Contracts.Data;

namespace Howestprime.Movies.Application.Contracts.Data
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