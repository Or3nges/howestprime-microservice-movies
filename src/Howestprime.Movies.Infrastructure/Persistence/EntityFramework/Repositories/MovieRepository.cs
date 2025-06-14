using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Howestprime.Movies.Domain.Movie;
using Microsoft.EntityFrameworkCore;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MoviesDbContext _context;

        public MovieRepository(MoviesDbContext context)
        {
            _context = context;
        }

        public async Task<Movie?> ById(MovieId id)
        {
            return await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Movie?> GetByIdAsync(MovieId id, CancellationToken cancellationToken = default)
        {
            return await _context.Movies.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task Save(Movie movie)
        {
            var existingMovie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == movie.Id);
            if (existingMovie == null)
            {
                await _context.Movies.AddAsync(movie);
            }
            else
            {
                _context.Movies.Update(movie);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Movie movie)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Movie>> FindByFiltersAsync(string? title, string? genre, CancellationToken cancellationToken = default)
        {
            var query = _context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(m => m.Title.Contains(title));
            }

            if (!string.IsNullOrWhiteSpace(genre))
            {
                query = query.Where(m => m.Genre.Contains(genre));
            }

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken = default)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync(cancellationToken);
            return movie;
        }
    }
}

