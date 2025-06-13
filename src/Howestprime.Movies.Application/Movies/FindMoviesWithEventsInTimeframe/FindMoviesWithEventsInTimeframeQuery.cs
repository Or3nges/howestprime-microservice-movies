using System;

namespace Howestprime.Movies.Application.Movies.FindMoviesWithEventsInTimeframe
{
    public class FindMoviesWithEventsInTimeframeQuery
    {
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
