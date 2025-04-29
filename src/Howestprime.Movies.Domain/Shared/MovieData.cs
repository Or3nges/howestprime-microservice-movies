namespace Howestprime.Movies.Domain.Shared
{
    public class MovieData
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Genres { get; set; }
        public string Actors { get; set; }
        public string AgeRating { get; set; }
        public int Duration { get; set; }
        public string PosterUrl { get; set; }
    }
}