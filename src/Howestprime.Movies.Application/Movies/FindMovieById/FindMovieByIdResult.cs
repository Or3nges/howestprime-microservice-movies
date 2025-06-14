using System;
using Howestprime.Movies.Domain.Movie;

namespace Howestprime.Movies.Application.UseCases.Movies.FindMovieById
{
    public class FindMovieByIdResult
    {
        public MovieId? Id { get; set; } 
        public string? PosterUrl { get; set; } 
        public string Title { get; set; } = string.Empty; 
        public string? Genre { get; set; } 
        public int AgeRating { get; set; } 
        public int Year { get; set; } 
        public int Duration { get; set; } 
        public string? Actors { get; set; }
        public string? Description { get; set; }
    }
}
