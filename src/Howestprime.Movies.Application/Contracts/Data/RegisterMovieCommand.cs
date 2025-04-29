namespace Howestprime.Movies.Application.Contracts.Data
{
    public class RegisterMovieCommand
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genres { get; set; }
        public string Actors { get; set; }
        public string AgeRating { get; set; }
        public int Duration { get; set; }
        public string PosterUrl { get; set; }
    }
}