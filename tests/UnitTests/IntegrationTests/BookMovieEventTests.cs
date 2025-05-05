using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Movies.BookMovieEvent;
using Howestprime.Movies.Domain.Entities;
using Howestprime.Movies.Domain.Events;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Repositories;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace UnitTests.IntegrationTests
{
    public class BookMovieEventTests
    {
        private MoviesDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseNpgsql("Host=localhost;Port=40021;Database=movies-howestprime;Username=devuser;Password=devpassword")
                .Options;
            return new MoviesDbContext(options);
        }

        private class DummyEventPublisher : IEventPublisher
        {
            public Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
            {
                return Task.CompletedTask;
            }

            public Task PublishAsync(BookingOpened bookingOpenedEvent)
            {
                return Task.CompletedTask;
            }
        }

        private IUnitOfWork CreateUnitOfWork(MoviesDbContext context)
        {
            var logger = new LoggerFactory().CreateLogger<EntityFrameworkUoW>();
            return new EntityFrameworkUoW(context, logger);
        }

        [Fact]
        public async Task BookMovieEvent_Success()
        {
            // Arrange
            var context = CreateDbContext();
            context.Database.EnsureCreated();
            
            var movieRepo = new MovieRepository(context);
            var movie = new Movie(Guid.NewGuid(), "Test Movie", "Test Description", "Action", "Test Actors", "12", 120, "poster.jpg");
            await movieRepo.AddAsync(movie);
            
            var room = new Room { Id = Guid.NewGuid(), Name = "Test Room", Capacity = 100 };
            context.Rooms.Add(room);
            
            await context.SaveChangesAsync();

            
            
            var movieEventRepo = new MovieEventRepository(context);
            var movieEvent = new MovieEvent
            {
                Id = Guid.NewGuid(),
                MovieId = movie.Id,
                RoomId = room.Id,
                Date = DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Utc),
                Time = new TimeSpan(18, 0, 0),
                Capacity = 50,
                Visitors = 0,
                Bookings = new List<Booking>()
            };
            await movieEventRepo.AddAsync(movieEvent);
            
            var unitOfWork = CreateUnitOfWork(context);
            var eventPublisher = new DummyEventPublisher();
            var handler = new BookMovieEventHandler(movieEventRepo, unitOfWork, eventPublisher);
            
            var command = new BookMovieEventCommand
            {
                MovieEventId = movieEvent.Id,
                StandardVisitors = 3,
                DiscountVisitors = 2,
                RoomName = room.Name
            };

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            Assert.NotEqual(Guid.Empty, result.BookingId);
            var updatedEvent = await context.MovieEvents
                .Include(e => e.Bookings)
                .FirstOrDefaultAsync(e => e.Id == movieEvent.Id);
                
            Assert.NotNull(updatedEvent);
            Assert.NotEmpty(updatedEvent.Bookings);
            Assert.Equal(5, updatedEvent.Visitors);
            
            var booking = updatedEvent.Bookings[0];
            Assert.Equal(3, booking.StandardVisitors);
            Assert.Equal(2, booking.DiscountVisitors);
        }

        [Fact]
        public async Task BookMovieEvent_NegativeVisitorCounts_ThrowsArgumentException()
        {
            // Arrange
            var context = CreateDbContext();
            context.Database.EnsureCreated();
            
            var movieEventRepo = new MovieEventRepository(context);
            var movieEvent = new MovieEvent
            {
                Id = Guid.NewGuid(),
                MovieId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Date = DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Utc),
                Time = new TimeSpan(15, 0, 0),
                Capacity = 50,
                Visitors = 0,
                Bookings = new List<Booking>()
            };
            await movieEventRepo.AddAsync(movieEvent);
            
            var unitOfWork = CreateUnitOfWork(context);
            var eventPublisher = new DummyEventPublisher();
            var handler = new BookMovieEventHandler(movieEventRepo, unitOfWork, eventPublisher);
            
            var command = new BookMovieEventCommand
            {
                MovieEventId = movieEvent.Id,
                StandardVisitors = -1,
                DiscountVisitors = 2,
                RoomName = "Room 1"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.HandleAsync(command));
        }

        [Fact]
        public async Task BookMovieEvent_ZeroVisitors_ThrowsArgumentException()
        {
            // Arrange
            var context = CreateDbContext();
            context.Database.EnsureCreated();
            
            var movieEventRepo = new MovieEventRepository(context);
            var movieEvent = new MovieEvent
            {
                Id = Guid.NewGuid(),
                MovieId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Date = DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Utc),
                Time = new TimeSpan(15, 0, 0),
                Capacity = 50,
                Visitors = 0,
                Bookings = new List<Booking>()
            };
            await movieEventRepo.AddAsync(movieEvent);
            
            var unitOfWork = CreateUnitOfWork(context);
            var eventPublisher = new DummyEventPublisher();
            var handler = new BookMovieEventHandler(movieEventRepo, unitOfWork, eventPublisher);
            
            var command = new BookMovieEventCommand
            {
                MovieEventId = movieEvent.Id,
                StandardVisitors = 0,
                DiscountVisitors = 0,
                RoomName = "Room 1"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => handler.HandleAsync(command));
        }

        [Fact]
        public async Task BookMovieEvent_NotEnoughCapacity_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = CreateDbContext();
            context.Database.EnsureCreated();
            
            var movieEventRepo = new MovieEventRepository(context);
            var movieEvent = new MovieEvent
            {
                Id = Guid.NewGuid(),
                MovieId = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                Date = DateTime.SpecifyKind(DateTime.Now.Date, DateTimeKind.Utc),
                Time = new TimeSpan(15, 0, 0),
                Capacity = 5,
                Visitors = 3,
                Bookings = new List<Booking>()
            };
            await movieEventRepo.AddAsync(movieEvent);
            
            var unitOfWork = CreateUnitOfWork(context);
            var eventPublisher = new DummyEventPublisher();
            var handler = new BookMovieEventHandler(movieEventRepo, unitOfWork, eventPublisher);
            
            var command = new BookMovieEventCommand
            {
                MovieEventId = movieEvent.Id,
                StandardVisitors = 2,
                DiscountVisitors = 1,
                RoomName = "Room 1"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
        }

        [Fact]
        public async Task BookMovieEvent_MovieEventNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var context = CreateDbContext();
            var movieEventRepo = new MovieEventRepository(context);
            var unitOfWork = CreateUnitOfWork(context);
            var eventPublisher = new DummyEventPublisher();
            var handler = new BookMovieEventHandler(movieEventRepo, unitOfWork, eventPublisher);
            
            var command = new BookMovieEventCommand
            {
                MovieEventId = Guid.NewGuid(),
                StandardVisitors = 2,
                DiscountVisitors = 1,
                RoomName = "Room 1"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
        }
    }
}
