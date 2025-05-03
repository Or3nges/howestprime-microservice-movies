using System;

namespace Howestprime.Movies.Domain.Entities
{
    public class Movie
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Genre { get; private set; }
        public string Actors { get; private set; }
        public string AgeRating { get; private set; }
        public int Duration { get; private set; }
        public string PosterUrl { get; private set; }

        public Movie(Guid id, string title, string description, string genre, string actors, string ageRating, int duration, string posterUrl)
        {
            Id = id;
            Title = title;
            Description = description;
            Genre = genre;
            Actors = actors;
            AgeRating = ageRating;
            Duration = duration;
            PosterUrl = posterUrl;
        }
    }
}
