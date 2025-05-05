// using System;
// using System.Linq;
// using System.Threading.Tasks;
// using Howestprime.Movies.Application.Contracts.Ports;
// using Howestprime.Movies.Application.Movies.FindMovieByIdWithEvents;
// using Howestprime.Movies.Domain.Entities;
// using Howestprime.Movies.Infrastructure.Persistence.EntityFramework;
// using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Repositories;
// using Microsoft.EntityFrameworkCore;
// using Xunit;

// namespace UnitTests.IntegrationTests
// {
//     public class FindMovieByIdWithEventsTests
//     {
//         private MoviesDbContext CreateDbContext()
//         {
//             var options = new DbContextOptionsBuilder<MoviesDbContext>()
//                 .UseNpgsql("Host=localhost;Port=40021;Database=movies-howestprime;Username=devuser;Password=devpassword")
//                 .Options;
//             return new MoviesDbContext(options);
//         }

//         [Fact]
//         public async Task ReturnsMovieWithEventsInNext14Days()
//         {
//             var context = CreateDbContext();
//             context.Database.EnsureCreated();
//             var movieRepo = new MovieRepository(context);
//             var roomRepo = new RoomRepository(context);
//             await roomRepo.SeedRoomsAsync();
//             var eventRepo = new MovieEventRepository(context);
//             var useCase = new FindMovieByIdWithEventsUseCase(movieRepo, eventRepo, roomRepo);

//             var movie = new Movie(Guid.NewGuid(), "EventTest", "desc", "genre", "actors", "12", 120, "poster");
//             await movieRepo.AddAsync(movie);
//             var room = context.Rooms.First();

//             var movieEvent = new MovieEvent
//             {
//                 Id = Guid.NewGuid(),
//                 MovieId = movie.Id,
//                 RoomId = room.Id,
//                 Date = new DateTime(2025, 5, 10, 0, 0, 0, DateTimeKind.Utc),
//                 Time = new TimeSpan(15, 0, 0),
//                 Capacity = 50
//             };
//             await eventRepo.AddAsync(movieEvent);

//             // Act
//             var query = new FindMovieByIdWithEventsQuery { MovieId = movie.Id };
//             var result = await useCase.ExecuteAsync(query);

//             // Assert
//             Assert.Equal(movie.Id, result.Id);
//             Assert.Single(result.Events);
//             Assert.Equal(movieEvent.Id, result.Events[0].Id);
//         }

//         [Fact]
//         public async Task Returns404IfNoEventsInNext14Days()
//         {
//             var context = CreateDbContext();
//             var movieRepo = new MovieRepository(context);
//             var roomRepo = new RoomRepository(context);
//             await roomRepo.SeedRoomsAsync();
//             var eventRepo = new MovieEventRepository(context);
//             var useCase = new FindMovieByIdWithEventsUseCase(movieRepo, eventRepo, roomRepo);

//             var movie = new Movie(Guid.NewGuid(), "EventTest", "desc", "genre", "actors", "12", 120, "poster");
//             await movieRepo.AddAsync(movie);
//             var room = context.Rooms.First();

//             var movieEvent = new MovieEvent
//             {
//                 Id = Guid.NewGuid(),
//                 MovieId = movie.Id,
//                 RoomId = room.Id,
//                 Date = new DateTime(2025, 5, 30, 0, 0, 0, DateTimeKind.Utc),
//                 Time = new TimeSpan(15, 0, 0),
//                 Capacity = 50
//             };
//             await eventRepo.AddAsync(movieEvent);

//             var query = new FindMovieByIdWithEventsQuery { MovieId = movie.Id };
//             await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(query));
//         }

//         [Fact]
//         public async Task Returns404IfMovieNotFound()
//         {
//             var context = CreateDbContext();
//             var movieRepo = new MovieRepository(context);
//             var roomRepo = new RoomRepository(context);
//             await roomRepo.SeedRoomsAsync();
//             var eventRepo = new MovieEventRepository(context);
//             var useCase = new FindMovieByIdWithEventsUseCase(movieRepo, eventRepo, roomRepo);

//             var query = new FindMovieByIdWithEventsQuery { MovieId = Guid.NewGuid() };
//             await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(query));
//         }
//     }
// }
