using Microsoft.EntityFrameworkCore;
using Howestprime.Movies.Domain.Entities;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework
{
    public class MoviesDbContext : DbContext
    {
        public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }

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
        }
    }
}
