using Microsoft.EntityFrameworkCore;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Domain.MovieEvent;
using Howestprime.Movies.Domain.Room;
using Howestprime.Movies.Domain.Booking;
using System;
using System.Text.Json;
using System.Collections.Generic;
using Domaincrafters.Domain;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework
{
    public class MoviesDbContext : DbContext
    {
        public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieEvent> MovieEvents { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Movie configuration
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasConversion(
                        v => v.Value,
                        v => new MovieId(v));
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Year).IsRequired().HasColumnType("integer");
                entity.Property(e => e.Duration).IsRequired();
                entity.Property(e => e.Genre).IsRequired();
                entity.Property(e => e.Actors).IsRequired();
                entity.Property(e => e.AgeRating).IsRequired();
                entity.Property(e => e.PosterUrl).IsRequired();
            });

            // MovieEvent configuration
            modelBuilder.Entity<MovieEvent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasConversion(
                        v => v.Value,
                        v => new MovieEventId(v));
                entity.Property(e => e.MovieId)
                    .HasConversion(
                        v => v.Value,
                        v => new MovieId(v));
                entity.Property(e => e.RoomId)
                    .HasConversion(
                        v => v.Value,
                        v => new RoomId(v));
                entity.Property(e => e.Time).IsRequired();
                entity.Property(e => e.Capacity).IsRequired();

                entity.HasOne<Movie>()
                    .WithMany()
                    .HasForeignKey(e => e.MovieId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Room>()
                    .WithMany()
                    .HasForeignKey(e => e.RoomId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Room configuration
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasConversion(
                        v => v.Value,
                        v => new RoomId(v));
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Capacity).IsRequired();
            });

            // Booking configuration
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasConversion(
                        v => v.Value,
                        v => new BookingId(v));
                
                entity.Property(e => e.MovieEventId)
                    .HasConversion(
                        v => v.Value,
                        v => new MovieEventId(v));
                
                entity.HasOne(b => b.MovieEvent)
                    .WithMany(me => me.Bookings)
                    .HasForeignKey(b => b.MovieEventId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.Property(e => e.StandardVisitors).IsRequired();
                entity.Property(e => e.DiscountVisitors).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.PaymentStatus).IsRequired();
                entity.Property(e => e.RoomName).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.SeatNumbers)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions)null));
            });
        }
    }
}
