using System;
using Domaincrafters.Domain;

namespace Howestprime.Movies.Domain.Entities
{

public sealed class MovieId(string? id="") : UuidEntityId(id);

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

        public Movie(Guid id, string title, string description, string genre, string actors, string ageRating, int duration, string posterUrl)
        {
            Title = title;
            Description = description;
            Genre = genre;
            Year = year;
            Actors = actors;
            AgeRating = ageRating;
            Duration = duration;
            PosterUrl = posterUrl;
        }

        public static Movie Create(string title, string description, int year, int duration, string genre, string actors, int ageRating, string posterUrl,MovieId? movieId = null){
            Movie movie = new Movie(movieId ?? new MovieId(), title, description, year, genre, duration, ageRating, posterUrl);
            movie.ValidState();

            DomainEventPublisher
            .Instance
            .Publish (MovieRegistered.Create(movie.Id.Value, title, description, year, duration, genre, actors, ageRating, posterUrl));
            return movie;
        }

        public override void ValidState(){
            EnsureTitleIsValid(Title);
            EnsureDescriptionIsValid(Description);
            EnsureYearIsValid(Year);

            EnsureGenreIsValid(Genre);

            EnsureDurationIsValid(Duration);

            EnsureActorsIsValid(Actors);

            EnsureAgeRatingIsValid(AgeRating);

            EnsurePosterUrlIsValid(PosterUrl);

        }
        private static void EnsureTitleIsValid(string title){
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("Title cannot be empty");
            }
            else if (title.Length > 250){
                throw new ArgumentException("Title cannot be longer than 250 characters");
            }
        }

            private static void EnsureDescriptionIsValid(string description){
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException("Description cannot be empty");
            }
            else if (title.Length > 500){
                throw new ArgumentException("Description cannot be longer than 500 characters");
            }

            private static void EnsureYearIsValid(string year){
            if (year > DateTime.Now.Year + 1)
            {
                throw new ArgumentException("Year can only be 1 year in the future");
            }
            else if (year < 1990){
                throw new ArgumentException("Year cannot be less than 1990");
            }
            }

            private static void EnsureDurationIsValid(int duration){
            if (duration < 0)
            {
                throw new ArgumentException("Duration cannot be negative");
            }
            else if (duration > 300){
                throw new ArgumentException("Duration cannot be higher than 300 minutes");
            }

            private static void EnsureActorsIsValid(int actors){
            if (string.IsNullOrEmpty(actors))
            {
                throw new ArgumentException("Actors cannot be Empty");
            }
            else if (actors.Length > 250){
                throw new ArgumentException("Actors cannot be longer than 250 characters");
            }
        }
            private static void EnsureAgeRatingIsValid(int duration){
            if (string.IsNullOrEmpty(actors))
            {
                throw new ArgumentException("Actors cannot be Empty");
            }
            else if (actors.Length > 250){
                throw new ArgumentException("Actors cannot be longer than 250 characters");
            }

    }

}