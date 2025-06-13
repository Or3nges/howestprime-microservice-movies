using System;
using Domaincrafters.Domain;

namespace Howestprime.Movies.Domain.Entities
{
    public sealed class MovieId : UuidEntityId
    {
        public MovieId(string? id = "") : base(id)
        {
        }
    }

    public class Movie : Entity<MovieId>
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int Year { get; private set; }
        public string Genre { get; private set; }
        public string Actors { get; private set; }
        public string AgeRating { get; private set; }
        public int Duration { get; private set; }
        public string PosterUrl { get; private set; }

        public Movie(MovieId id, string title, string description, int year, string genre, string actors, string ageRating, int duration, string posterUrl)
            : base(id)
        {
            Title = title;
            Description = description;
            Year = year;
            Genre = genre;
            Actors = actors;
            AgeRating = ageRating;
            Duration = duration;
            PosterUrl = posterUrl;
        }

        public static Movie Create(string title, string description, int year, int duration, string genre, string actors, string ageRating, string posterUrl, MovieId? movieId = null)
        {
            Movie movie = new Movie(movieId ?? new MovieId(), title, description, year, genre, actors, ageRating, duration, posterUrl);
            movie.ValidateState();

            return movie;
        }

        public override void ValidateState()
        {
            EnsureTitleIsValid(Title);
            EnsureDescriptionIsValid(Description);
            EnsureYearIsValid(Year);
            EnsureDurationIsValid(Duration);
            EnsureActorsIsValid(Actors);
            EnsureAgeRatingIsValid(AgeRating);
            EnsurePosterUrlIsValid(PosterUrl);
        }

        private static void EnsureTitleIsValid(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("Title cannot be empty");
            }
            else if (title.Length > 250)
            {
                throw new ArgumentException("Title cannot be longer than 250 characters");
            }
        }

        private static void EnsureDescriptionIsValid(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException("Description cannot be empty");
            }
            else if (description.Length > 500)
            {
                throw new ArgumentException("Description cannot be longer than 500 characters");
            }
        }

        private static void EnsureYearIsValid(int year)
        {
            if (year > DateTime.Now.Year + 1)
            {
                throw new ArgumentException("Year can only be 1 year in the future");
            }
            else if (year < 1990)
            {
                throw new ArgumentException("Year cannot be less than 1990");
            }
        }

        private static void EnsureDurationIsValid(int duration)
        {
            if (duration < 0)
            {
                throw new ArgumentException("Duration cannot be negative");
            }
            else if (duration > 300)
            {
                throw new ArgumentException("Duration cannot be higher than 300 minutes");
            }
        }

        private static void EnsureActorsIsValid(string actors)
        {
            if (string.IsNullOrEmpty(actors))
            {
                throw new ArgumentException("Actors cannot be empty");
            }
            else if (actors.Length > 250)
            {
                throw new ArgumentException("Actors cannot be longer than 250 characters");
            }
        }

        private static void EnsureAgeRatingIsValid(string ageRating)
        {
            if (string.IsNullOrEmpty(ageRating))
            {
                throw new ArgumentException("AgeRating cannot be empty");
            }
            else if (ageRating.Length > 250)
            {
                throw new ArgumentException("AgeRating cannot be longer than 250 characters");
            }
        }

        private static void EnsurePosterUrlIsValid(string posterUrl)
        {
            if (string.IsNullOrEmpty(posterUrl))
            {
                throw new ArgumentException("PosterUrl cannot be empty");
            }
        }
    }
}
