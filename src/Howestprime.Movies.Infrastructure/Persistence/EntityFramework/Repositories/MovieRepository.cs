using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Entities;
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

        public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Movies.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken = default)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync(cancellationToken);
            return movie;
        }

        public async Task<IEnumerable<Movie>> FindByFiltersAsync(string? title, string? genre, CancellationToken cancellationToken = default)
        {
            var query = _context.Movies.AsQueryable();
            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(m => m.Title.Contains(title));
            if (!string.IsNullOrWhiteSpace(genre))
                query = query.Where(m => m.Genre.Contains(genre));
            return await query.ToListAsync(cancellationToken);
        }
    }
}
