using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests.Shared
{
    public class FakeMovieRepository : IMovieRepository
    {
        public List<Movie> Movies { get; set; } = new List<Movie>();

        public Task<Movie?> GetByIdAsync(MovieId id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Movies.FirstOrDefault(m => m.Id == id));
        }

        public Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken = default)
        {
            Movies.Add(movie);
            return Task.FromResult(movie);
        }

        public Task<IEnumerable<Movie>> FindByFiltersAsync(string? title, string? genre, CancellationToken cancellationToken = default)
        {
            var result = Movies.AsEnumerable();
            if (!string.IsNullOrEmpty(title))
            {
                result = result.Where(m => m.Title.Contains(title, StringComparison.InvariantCultureIgnoreCase));
            }
            if (!string.IsNullOrEmpty(genre))
            {
                result = result.Where(m => m.Genre.Equals(genre, StringComparison.InvariantCultureIgnoreCase));
            }
            return Task.FromResult(result.ToList() as IEnumerable<Movie>);
        }

        public Task<Movie?> ById(MovieId id)
        {
            return Task.FromResult(Movies.FirstOrDefault(m => m.Id == id));
        }

        public Task Save(Movie movie)
        {
            return Task.CompletedTask;
        }

        public Task Remove(Movie movie)
        {
             Movies.Remove(movie);
            return Task.CompletedTask;
        }
    }

    public class FakeMovieEventRepository : IMovieEventRepository
    {
        public List<MovieEvent> Events { get; set; } = new List<MovieEvent>();

        public Task AddAsync(MovieEvent movieEvent)
        {
            Events.Add(movieEvent);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(MovieEventId id)
        {
            Events.RemoveAll(e => e.Id == id);
            return Task.CompletedTask;
        }

        public Task<MovieEvent> GetByIdWithBookingsAsync(MovieEventId movieEventId)
        {
            return Task.FromResult(Events.FirstOrDefault(e => e.Id == movieEventId)!);
        }

        public Task<IEnumerable<MovieEvent>> GetEventsForMovieInRangeAsync(MovieId movieId, DateTime start, DateTime end)
        {
            return Task.FromResult(Events.Where(e => e.MovieId == movieId && e.Time >= start && e.Time <= end));
        }

        public Task<MovieEvent?> GetByRoomDateTimeAsync(RoomId roomId, DateTime date, TimeSpan time)
        {
            return Task.FromResult(Events.FirstOrDefault(e => e.RoomId == roomId && e.Time.Date == date.Date && e.Time.TimeOfDay == time));
        }
        
        public Task UpdateAsync(MovieEvent movieEvent)
        {
            var index = Events.FindIndex(e => e.Id == movieEvent.Id);
            if (index != -1)
            {
                Events[index] = movieEvent;
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<MovieEvent>> GetEventsInRangeAsync(DateTime start, DateTime end)
        {
            return Task.FromResult(Events.Where(e => e.Time >= start && e.Time <= end));
        }
    }

    public class FakeRoomRepository : IRoomRepository
    {
        public List<Room> Rooms { get; set; } = new List<Room>();

        public Task<Room> AddAsync(Room room)
        {
            Rooms.Add(room);
            return Task.FromResult(room);
        }

        public Task<Room?> ById(RoomId id)
        {
            return Task.FromResult(Rooms.FirstOrDefault(r => r.Id == id));
        }

        public Task<IEnumerable<Room>> GetAllAsync()
        {
            return Task.FromResult(Rooms.AsEnumerable());
        }

        public Task<Room?> GetByIdAsync(RoomId id)
        {
            return Task.FromResult(Rooms.FirstOrDefault(r => r.Id == id));
        }
        
        public Task SeedRoomsAsync()
        {
            return Task.CompletedTask;
        }
    }
} 