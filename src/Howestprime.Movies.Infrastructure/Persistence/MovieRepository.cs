using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Entities;

namespace Howestprime.Movies.Infrastructure.Persistence
{
    public class MovieRepository : IMovieRepository
    {
        private static readonly ConcurrentDictionary<MovieId, Movie> _movies = new();

        public Task<Movie?> GetByIdAsync(MovieId id, CancellationToken cancellationToken = default)
        {
            _movies.TryGetValue(id, out var movie);
            return Task.FromResult(movie);
        }

        public Task<Movie?> ById(MovieId id)
        {
            _movies.TryGetValue(id, out var movie);
            return Task.FromResult(movie);
        }

        public Task Save(Movie movie)
        {
            _movies[movie.Id] = movie;
            return Task.CompletedTask;
        }

        public Task Remove(Movie movie)
        {
            _movies.TryRemove(movie.Id, out _);
            return Task.CompletedTask;
        }

        public Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken = default)
        {
            _movies[movie.Id] = movie;
            return Task.FromResult(movie);
        }

        public Task<IEnumerable<Movie>> FindByFiltersAsync(string? title, string? genre, CancellationToken cancellationToken = default)
        {
            var result = _movies.Values.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(title))
                result = result.Where(m => m.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(genre))
                result = result.Where(m => m.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(result);
        }
    }
}
