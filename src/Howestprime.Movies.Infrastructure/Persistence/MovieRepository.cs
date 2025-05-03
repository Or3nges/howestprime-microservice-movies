using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Entities;

namespace Howestprime.Movies.Infrastructure.Persistence
{
    public class MovieRepository : IMovieRepository
    {
        private static readonly ConcurrentDictionary<Guid, Movie> _movies = new();

        public Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _movies.TryGetValue(id, out var movie);
            return Task.FromResult(movie);
        }

        public Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken = default)
        {
            _movies[movie.Id] = movie;
            return Task.FromResult(movie);
        }
    }
}
