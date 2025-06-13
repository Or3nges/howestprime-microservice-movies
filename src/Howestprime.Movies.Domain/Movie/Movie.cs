using System;
using System.Collections.Generic;
using Domaincrafters.Domain;
using Howestprime.Movies.Domain.MovieEvent;
using Howestprime.Movies.Domain.Shared; // For MoviesDomainEvent, if MovieRegistered is published here
using Howestprime.Movies.Domain.Entities;
using Howestprime.Movies.Domain.Events;

namespace Howestprime.Movies.Domain.Movie // Changed namespace
{

    public class Movie : Entity<MovieId> // Inherits from Entity<MovieId>
    {
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public int Year { get; private set; }
        public string? Genre { get; private set; }
        public int Duration { get; private set; }        public string? Actors { get; private set; }
        public int AgeRating { get; private set; } // Changed from string? to int
        public string? PosterUrl { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public List<Howestprime.Movies.Domain.MovieEvent.MovieEvent> Events { get; set; } = new List<Howestprime.Movies.Domain.MovieEvent.MovieEvent>();

        // Private constructor
        private Movie(MovieId id, string title, string? description, int year, string? genre, int duration, string? actors, int ageRating, string? posterUrl, DateTime createdAt, DateTime updatedAt) : base(id)
        {
            Title = title;
            Description = description;
            Year = year;
            Genre = genre;
            Duration = duration;
            Actors = actors;
            AgeRating = ageRating; // Changed
            PosterUrl = posterUrl;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        // Public static Create factory method
        public static Movie Create(
            string title, 
            string? description, 
            int year, 
            string? genre, 
            int duration, 
            string? actors, 
            int ageRating, // Changed from string? to int
            string? posterUrl)        {
            var id = new MovieId(Guid.NewGuid().ToString());
            var now = DateTime.UtcNow;
            var movie = new Movie(id, title, description, year, genre, duration, actors, ageRating, posterUrl, now, now); // Changed

            movie.ValidateState();
            
            // Domain Event Publishing
            DomainEventPublisher.Instance.Publish(
                MovieRegistered.Create(
                    movie.Id.Value,
                    movie.Title,
                    movie.Year,
                    movie.Duration,
                    movie.Genre,
                    movie.Actors,
                    movie.AgeRating, // Changed
                    movie.PosterUrl));
            
            return movie;
        }

        public void UpdateDetails(
            string title,
            string? description,
            int year,
            string? genre,
            int duration,
            string? actors,
            int ageRating, // Changed from string? to int
            string? posterUrl)
        {
            Title = title;
            Description = description;
            Year = year;
            Genre = genre;
            Duration = duration;
            Actors = actors;
            AgeRating = ageRating; // Changed
            PosterUrl = posterUrl;
            UpdatedAt = DateTime.UtcNow;

            ValidateState();
            // Consider publishing a MovieDetailsChanged event, aligning with your spec
            // DomainEventPublisher.Instance.Publish(MovieDetailsChanged.Create(...));
        }

        public override void ValidateState() // Renamed from ValidState
        {
            EnsureTitleIsValid(Title);
            EnsureDescriptionIsValid(Description);
            EnsureYearIsValid(Year);
            EnsureGenreIsValid(Genre);
            EnsureDurationIsValid(Duration);
            EnsureActorsAreValid(Actors); // Renamed for clarity
            EnsureAgeRatingIsValid(AgeRating);
            EnsurePosterUrlIsValid(PosterUrl);
        }

        private static void EnsureTitleIsValid(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty or whitespace.");
            }
            if (title.Length > 250)
            {
                throw new ArgumentException("Title cannot be longer than 250 characters.");
            }
        }

        private static void EnsureDescriptionIsValid(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description cannot be empty or whitespace.");
            }
            if (description.Length > 1000) // Assuming a max length for description
            {
                throw new ArgumentException("Description cannot be longer than 1000 characters.");
            }
        }

        private static void EnsureYearIsValid(int year) // Changed param type to int
        {
            if (year > DateTime.Now.Year + 5) // Allow a bit more future leeway, adjust as needed
            {
                throw new ArgumentException($"Year cannot be more than {DateTime.Now.Year + 5}.");
            }
            if (year < 1888) // Earliest known films
            {
                throw new ArgumentException("Year cannot be less than 1888.");
            }
        }
        
        private static void EnsureGenreIsValid(string? genre)
        {
            if (string.IsNullOrWhiteSpace(genre))
            {
                throw new ArgumentException("Genre cannot be empty or whitespace.");
            }
            if (genre.Length > 100)
            {
                throw new ArgumentException("Genre cannot be longer than 100 characters.");
            }
        }

        private static void EnsureDurationIsValid(int duration)
        {
            if (duration <= 0)
            {
                throw new ArgumentException("Duration must be positive.");
            }
            if (duration > 600) // e.g., 10 hours, adjust as needed
            {
                throw new ArgumentException("Duration cannot be higher than 600 minutes.");
            }
        }

        private static void EnsureActorsAreValid(string? actors) // Changed param type to string, method name
        {
            if (string.IsNullOrWhiteSpace(actors))
            {
                throw new ArgumentException("Actors cannot be empty or whitespace.");
            }
            if (actors.Length > 500) 
            {
                throw new ArgumentException("Actors list cannot be longer than 500 characters.");
            }
        }

        private static void EnsureAgeRatingIsValid(int ageRating) // Changed from string to int
        {
            // Assuming age rating must be non-negative. Adjust if specific values (e.g., 0, 6, 12, 15, 18) are required.
            if (ageRating < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ageRating), "Age rating cannot be negative.");
            }
            // Example: if only specific ratings are allowed:
            // var allowedRatings = new[] { 0, 6, 12, 15, 18 };
            // if (!allowedRatings.Contains(ageRating))
            // {
            //     throw new ArgumentOutOfRangeException(nameof(ageRating), $"Invalid age rating. Allowed values are: {string.Join(", ", allowedRatings)}.");
            // }
        }
        
        private static void EnsurePosterUrlIsValid(string? posterUrl)
        {
            if (string.IsNullOrWhiteSpace(posterUrl))
            {
                // Poster URL can be optional depending on requirements
                // throw new ArgumentException("Poster URL cannot be empty or whitespace.");
                return; 
            }
            if (!Uri.TryCreate(posterUrl, UriKind.Absolute, out _))
            {
                throw new ArgumentException("Poster URL must be a valid absolute URI.");
            }
            if (posterUrl.Length > 2048) // Standard URL length limit
            {
                throw new ArgumentException("Poster URL cannot be longer than 2048 characters.");
            }
        }
    }
}