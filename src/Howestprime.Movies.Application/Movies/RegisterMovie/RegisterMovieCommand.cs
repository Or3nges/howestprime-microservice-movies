namespace Howestprime.Movies.Application.Movies.RegisterMovie
{
    public class RegisterMovieCommand
    {
        public string? Title { get; set; } // Make nullable to fix warning
        public string? Description { get; set; }
        public string? Genre { get; set; }
        public string? Actors { get; set; }
        public string? AgeRating { get; set; }
        public int Duration { get; set; }
        public string? PosterUrl { get; set; }
    }
}