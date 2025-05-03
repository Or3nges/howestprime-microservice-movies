namespace Howestprime.Movies.Application.DTO;

public class RegisterMovieRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Genre { get; set; }
    public string? Actors { get; set; }
    public int AgeRating { get; set; }
    public int Duration { get; set; }
    public string? PosterUrl { get; set; }
}