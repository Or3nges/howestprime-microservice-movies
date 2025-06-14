using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Events;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Domain.MovieEvent;
using Howestprime.Movies.Domain.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domaincrafters.Domain;

namespace UnitTests.Shared
{
    public class FakeMovieRepository : IMovieRepository
    {
        private readonly List<Movie> _movies = new();
        public IReadOnlyList<Movie> Movies => _movies;

        public void SetTestData(IEnumerable<Movie> movies)
        {
            _movies.Clear();
            _movies.AddRange(movies);
        }

        public Task<Movie?> GetByIdAsync(MovieId id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_movies.FirstOrDefault(m => m.Id == id));
        }

        public Task<Movie> AddAsync(Movie movie, CancellationToken cancellationToken = default)
        {
            _movies.Add(movie);
            return Task.FromResult(movie);
        }

        public Task<IEnumerable<Movie>> FindByFiltersAsync(string? title, string? genre, CancellationToken cancellationToken = default)
        {
            var result = _movies.AsEnumerable();
            if (!string.IsNullOrEmpty(title))
            {
                result = result.Where(m => m.Title.Contains(title, StringComparison.InvariantCultureIgnoreCase));
            }
            if (!string.IsNullOrEmpty(genre))
            {
                result = result.Where(m => m.Genre.Equals(genre, StringComparison.InvariantCultureIgnoreCase));
            }
            return Task.FromResult(result);
        }

        public Task<Movie?> ById(MovieId id)
        {
            return Task.FromResult(_movies.FirstOrDefault(m => m.Id == id));
        }

        public Task Save(Movie movie)
        {
            var existing = _movies.FirstOrDefault(m => m.Id == movie.Id);
            if (existing != null)
            {
                _movies.Remove(existing);
            }
            _movies.Add(movie);
            return Task.CompletedTask;
        }

        public Task Remove(Movie movie)
        {
            _movies.Remove(movie);
            return Task.CompletedTask;
        }
    }

    public class FakeMovieEventRepository : IMovieEventRepository
    {
        private readonly List<MovieEvent> _events = new();
        public IReadOnlyList<MovieEvent> Events => _events;

        public void SetTestData(IEnumerable<MovieEvent> events)
        {
            _events.Clear();
            _events.AddRange(events);
        }

        public Task AddAsync(MovieEvent movieEvent)
        {
            _events.Add(movieEvent);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(MovieEventId id)
        {
            _events.RemoveAll(e => e.Id == id);
            return Task.CompletedTask;
        }

        public Task<MovieEvent> GetByIdWithBookingsAsync(MovieEventId movieEventId)
        {
            return Task.FromResult(_events.FirstOrDefault(e => e.Id == movieEventId)!);
        }

        public Task<IEnumerable<MovieEvent>> GetEventsForMovieInRangeAsync(MovieId movieId, DateTime start, DateTime end)
        {
            return Task.FromResult(_events.Where(e => e.MovieId == movieId && e.Time >= start && e.Time <= end));
        }

        public Task<MovieEvent?> GetByRoomDateTimeAsync(RoomId roomId, DateTime date, TimeSpan time)
        {
            return Task.FromResult(_events.FirstOrDefault(e => e.RoomId == roomId && e.Time.Date == date.Date && e.Time.TimeOfDay == time));
        }
        
        public Task UpdateAsync(MovieEvent movieEvent)
        {
            var index = _events.FindIndex(e => e.Id == movieEvent.Id);
            if (index != -1)
            {
                _events[index] = movieEvent;
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<MovieEvent>> GetEventsInRangeAsync(DateTime start, DateTime end)
        {
            return Task.FromResult(_events.Where(e => e.Time >= start && e.Time <= end));
        }

        public Task<MovieEvent?> GetByIdAsync(MovieEventId id)
        {
            return Task.FromResult(_events.FirstOrDefault(e => e.Id == id));
        }

        public Task Save(MovieEvent movieEvent)
        {
            var existing = _events.FirstOrDefault(e => e.Id == movieEvent.Id);
            if (existing != null)
            {
                _events.Remove(existing);
            }
            _events.Add(movieEvent);
            return Task.CompletedTask;
        }
    }

    public class FakeRoomRepository : IRoomRepository
    {
        private readonly List<Room> _rooms = new();
        public IReadOnlyList<Room> Rooms => _rooms;

        public void SetTestData(IEnumerable<Room> rooms)
        {
            _rooms.Clear();
            _rooms.AddRange(rooms);
        }

        public Task<Room> AddAsync(Room room)
        {
            _rooms.Add(room);
            return Task.FromResult(room);
        }

        public Task<Room?> ById(RoomId id)
        {
            return Task.FromResult(_rooms.FirstOrDefault(r => r.Id == id));
        }

        public Task<IEnumerable<Room>> GetAllAsync()
        {
            return Task.FromResult(_rooms.AsEnumerable());
        }

        public Task<Room?> GetByIdAsync(RoomId id)
        {
            return Task.FromResult(_rooms.FirstOrDefault(r => r.Id == id));
        }
        
        public Task SeedRoomsAsync()
        {
            return Task.CompletedTask;
        }

        public Task Save(Room room)
        {
            var existing = _rooms.FirstOrDefault(r => r.Id == room.Id);
            if (existing != null)
            {
                _rooms.Remove(existing);
            }
            _rooms.Add(room);
            return Task.CompletedTask;
        }
    }

    public class FakeUnitOfWork : IUnitOfWork
    {
        public bool Committed { get; private set; }

        public Task CommitAsync()
        {
            Committed = true;
            return Task.CompletedTask;
        }
    }

    public class FakeEventPublisher : IEventPublisher
    {
        public List<BookingOpened> PublishedEvents { get; } = new();

        public Task PublishAsync(BookingOpened bookingOpenedEvent)
        {
            PublishedEvents.Add(bookingOpenedEvent);
            return Task.CompletedTask;
        }
    }

    public class FakeDomainEventSubscriber : IDomainEventSubscriber
    {
        public List<IDomainEvent> HandledEvents { get; } = new();

        public void HandleEvent(IDomainEvent domainEvent)
        {
            HandledEvents.Add(domainEvent);
        }

        public Type SubscribedToEventType => typeof(IDomainEvent);
        
        public bool IsSubscribedTo(IDomainEvent domainEvent)
        {
            return true;
        }
    }
} 