using System;
using Domaincrafters.Domain;

namespace Howestprime.Movies.Domain.Events
{
    public class MovieRegistered : IDomainEvent
    {
        public string MovieId { get; private set; }
        public string Title { get; private set; }
        public int Year { get; private set; }
        public int Duration { get; private set; }
        public string Genre { get; private set; }
        public string Actors { get; private set; }
        public int AgeRating { get; private set; }
        public string PosterUrl { get; private set; }
        public string QualifiedEventName => "MovieRegistered";
        public DateTime OccurredOn { get; private set; }

        private MovieRegistered(string movieId, string title, int year, int duration, string genre, string actors, int ageRating, string posterUrl)
        {
            MovieId = movieId;
            Title = title;
            Year = year;
            Duration = duration;
            Genre = genre;
            Actors = actors;
            AgeRating = ageRating;
            PosterUrl = posterUrl;
            OccurredOn = DateTime.UtcNow;
        }

        public static MovieRegistered Create(string movieId, string title, int year, int duration, string genre, string actors, int ageRating, string posterUrl)
        {
            return new MovieRegistered(movieId, title, year, duration, genre, actors, ageRating, posterUrl);
        }
    }
} 