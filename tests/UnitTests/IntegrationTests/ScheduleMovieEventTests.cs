// using System;
// using System.Linq;
// using System.Threading.Tasks;
// using Howestprime.Movies.Application.Contracts.Ports;
// using Howestprime.Movies.Application.Movies.RegisterMovie;
// using Howestprime.Movies.Application.Movies.ScheduleMovieEvent;
// using Howestprime.Movies.Domain.Entities;
// using Howestprime.Movies.Infrastructure.Persistence.EntityFramework;
// using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Repositories;
// using Microsoft.EntityFrameworkCore;
// using Xunit;

// namespace UnitTests.IntegrationTests
// {
//     public class ScheduleMovieEventTests
//     {
//         private MoviesDbContext CreateDbContext()
//         {
//             var options = new DbContextOptionsBuilder<MoviesDbContext>()
//                 .UseNpgsql("Host=localhost;Port=40021;Database=movies-howestprime;Username=devuser;Password=devpassword")
//                 .Options;
//             return new MoviesDbContext(options);
//         }

//         [Fact]
//         public async Task ScheduleMovieEvent_Success()
//         {
//             // Arrange
//             var context = CreateDbContext();
//             context.Database.EnsureCreated();
//             var movieRepo = new MovieRepository(context);
//             var roomRepo = new RoomRepository(context);
//             await roomRepo.SeedRoomsAsync();
//             var eventRepo = new MovieEventRepository(context);
//             var useCase = new ScheduleMovieEventUseCase(movieRepo, eventRepo, roomRepo);

//             var movie = new Movie(Guid.NewGuid(), "EventTest", "desc", "genre", "actors", "12", 120, "poster");
//             await movieRepo.AddAsync(movie);



//             var room = context.Rooms.First();

//             // Act
//             var command = new ScheduleMovieEventCommand
//             {
//                 MovieId = movie.Id,
//                 RoomId = room.Id,
//                 StartDate = DateTime.UtcNow.Date.AddDays(1).Add(new TimeSpan(15, 0, 0)),
//                 Capacity = 100,
//                 Visitors = 0
//             };
//             var result = await useCase.ExecuteAsync(command);

//             // Assert
//             Assert.NotEqual(Guid.Empty, result.EventId);
//             var persisted = await context.MovieEvents.FindAsync(result.EventId);
//             Assert.NotNull(persisted);
//             Assert.Equal(movie.Id, persisted.MovieId);
//             Assert.Equal(room.Id, persisted.RoomId);
//         }

//         [Fact]
//         public async Task ScheduleMovieEvent_InvalidMovie_Throws()
//         {
//             var context = CreateDbContext();
//             var movieRepo = new MovieRepository(context);
//             var roomRepo = new RoomRepository(context);
//             await roomRepo.SeedRoomsAsync();
//             var eventRepo = new MovieEventRepository(context);
//             var useCase = new ScheduleMovieEventUseCase(movieRepo, eventRepo, roomRepo);

//             var room = context.Rooms.First();
//             var command = new ScheduleMovieEventCommand
//             {
//                 MovieId = Guid.NewGuid(),
//                 RoomId = room.Id,
//                 StartDate = DateTime.UtcNow.Date.AddDays(1).Add(new TimeSpan(15, 0, 0)),
//                 Capacity = 100,
//                 Visitors = 0
//             };
//             await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(command));
//         }

//         [Fact]
//         public async Task ScheduleMovieEvent_InvalidTime_Throws()
//         {
//             var context = CreateDbContext();
//             var movieRepo = new MovieRepository(context);
//             var roomRepo = new RoomRepository(context);
//             await roomRepo.SeedRoomsAsync();
//             var eventRepo = new MovieEventRepository(context);
//             var useCase = new ScheduleMovieEventUseCase(movieRepo, eventRepo, roomRepo);

//             var movie = new Movie(Guid.NewGuid(), "EventTest", "desc", "genre", "actors", "12", 120, "poster");
//             await movieRepo.AddAsync(movie);
//             var room = context.Rooms.First();

//             var command = new ScheduleMovieEventCommand
//             {
//                 MovieId = movie.Id,
//                 RoomId = room.Id,
//                 StartDate = DateTime.UtcNow.Date.AddDays(1).Add(new TimeSpan(14, 0, 0)), // Invalid time
//                 Capacity = 100,
//                 Visitors = 0
//             };
//             await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(command));
//         }

//         [Fact]
//         public async Task ScheduleMovieEvent_PastDate_Throws()
//         {
//             var context = CreateDbContext();
//             var movieRepo = new MovieRepository(context);
//             var roomRepo = new RoomRepository(context);
//             await roomRepo.SeedRoomsAsync();
//             var eventRepo = new MovieEventRepository(context);
//             var useCase = new ScheduleMovieEventUseCase(movieRepo, eventRepo, roomRepo);

//             var movie = new Movie(Guid.NewGuid(), "EventTest", "desc", "genre", "actors", "12", 120, "poster");
//             await movieRepo.AddAsync(movie);
//             var room = context.Rooms.First();

//             var command = new ScheduleMovieEventCommand
//             {
//                 MovieId = movie.Id,
//                 RoomId = room.Id,
//                 StartDate = DateTime.UtcNow.Date.AddDays(-1).Add(new TimeSpan(15, 0, 0)), // Past date
//                 Capacity = 100,
//                 Visitors = 0
//             };
//             await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(command));
//         }

//         [Fact]
//         public async Task ScheduleMovieEvent_InvalidCapacity_Throws()
//         {
//             var context = CreateDbContext();
//             var movieRepo = new MovieRepository(context);
//             var roomRepo = new RoomRepository(context);
//             await roomRepo.SeedRoomsAsync();
//             var eventRepo = new MovieEventRepository(context);
//             var useCase = new ScheduleMovieEventUseCase(movieRepo, eventRepo, roomRepo);

//             var movie = new Movie(Guid.NewGuid(), "EventTest", "desc", "genre", "actors", "12", 120, "poster");
//             await movieRepo.AddAsync(movie);
//             var room = context.Rooms.First();

//             var command = new ScheduleMovieEventCommand
//             {
//                 MovieId = movie.Id,
//                 RoomId = room.Id,
//                 StartDate = DateTime.UtcNow.Date.AddDays(1).Add(new TimeSpan(15, 0, 0)),
//                 Capacity = 0, // Invalid capacity
//                 Visitors = 0
//             };
//             await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(command));
//         }
//     }
// }