using Microsoft.EntityFrameworkCore;
using Howestprime.Movies.Domain.Entities;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework
{
    public class MoviesDbContext : DbContext
    {
        public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieEvent> MovieEvents { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Title).IsRequired();
                entity.Property(m => m.Description);
                entity.Property(m => m.Genre);
                entity.Property(m => m.Actors);
                entity.Property(m => m.AgeRating);
                entity.Property(m => m.Duration);
                entity.Property(m => m.PosterUrl);
            });
            modelBuilder.Entity<MovieEvent>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MovieId).IsRequired();
                entity.Property(e => e.RoomId).IsRequired();
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Time).IsRequired();
                entity.Property(e => e.Capacity).IsRequired();
            });
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired();
                entity.Property(r => r.Capacity).IsRequired();
            });
        }
    }
}
