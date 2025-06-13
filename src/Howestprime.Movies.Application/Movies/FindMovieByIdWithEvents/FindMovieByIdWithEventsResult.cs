using System;
using System.Collections.Generic;
using Howestprime.Movies.Application.Movies.FindMovieByIdWithEvents;
using Howestprime.Movies.Domain.Entities;
using Howestprime.Movies.Domain.Movie;

namespace Howestprime.Movies.Application.UseCases.Movies.FindMovieByIdWithEvents
{
    public class FindMovieByIdWithEventsResult
    {
        public MovieId? Id { get; set; } 
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; } 
        public string Genre { get; set; } = string.Empty;
        public List<string> Actors { get; set; } = new List<string>();
        public string? PosterUrl { get; set; }
        public int AgeRating { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<MovieEventResultData> Events { get; set; } = new List<MovieEventResultData>();
    }
}
