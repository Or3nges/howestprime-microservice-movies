// using System;
// using System.Linq;
// using System.Threading.Tasks;
// using Howestprime.Movies.Application.Contracts.Ports;
// using Howestprime.Movies.Application.Movies.FindMoviesWithEventsInTimeframe;
// using Howestprime.Movies.Domain.Entities;
// using Howestprime.Movies.Infrastructure.Persistence.EntityFramework;
// using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Repositories;
// using Microsoft.EntityFrameworkCore;
// using Xunit;

// namespace UnitTests.IntegrationTests
// {
//     public class FindMoviesWithEventsInTimeframeTests
//     {
//         private MoviesDbContext CreateDbContext()
//         {
//             var options = new DbContextOptionsBuilder<MoviesDbContext>()
//                 .UseNpgsql("Host=localhost;Port=40021;Database=movies-howestprime;Username=devuser;Password=devpassword")
//                 .Options;
//             return new MoviesDbContext(options);
//         }

//         [Fact]
//         public async Task ReturnsMoviesWithEventsInNext14Days_FilteredByTitleAndGenre()
//         {
//             var context = CreateDbContext();
//             context.Database.EnsureCreated();
            
//             await context.SaveChangesAsync();
            
//             var movieRepo = new MovieRepository(context);
//             var roomRepo = new RoomRepository(context);
//             await roomRepo.SeedRoomsAsync();
//             var eventRepo = new MovieEventRepository(context);
//             var useCase = new FindMoviesWithEventsInTimeframeUseCase(movieRepo, eventRepo, roomRepo);

//             var movieId1 = Guid.NewGuid();
//             var movie1 = new Movie(movieId1, "Inception", "A mind-bending thriller", "Sci-Fi", "Leonardo DiCaprio", "12", 120, "poster.jpg");
//             var movieId2 = Guid.NewGuid();
//             var movie2 = new Movie(movieId2, "Titanic", "A romance drama", "Drama", "Leonardo DiCaprio", "12", 120, "poster2.jpg");
//             await movieRepo.AddAsync(movie1);
//             await movieRepo.AddAsync(movie2);
            
//             var room = context.Rooms.First();
//             Assert.NotNull(room);
            
//             var movieEvent1 = new MovieEvent
//             {
//                 Id = Guid.NewGuid(),
//                 MovieId = movieId1,
//                 RoomId = room.Id,
//                 Date = DateTime.UtcNow.Date.AddDays(2),
//                 Time = new TimeSpan(15, 0, 0),
//                 Capacity = 50,
//                 Visitors = 0,
//                 Bookings = new List<Booking>()
//             };
//             await eventRepo.AddAsync(movieEvent1);

//             var movieEvent2 = new MovieEvent
//             {
//                 Id = Guid.NewGuid(),
//                 MovieId = movieId2,
//                 RoomId = room.Id,
//                 Date = DateTime.UtcNow.Date.AddDays(3),
//                 Time = new TimeSpan(15, 0, 0),
//                 Capacity = 50,
//                 Visitors = 0,
//                 Bookings = new List<Booking>()
//             };
//             await eventRepo.AddAsync(movieEvent2);
            
//             var persistedMovies = await context.Movies.ToListAsync();
//             Assert.Contains(persistedMovies, m => m.Id == movieId1);
//             Assert.Contains(persistedMovies, m => m.Title == "Inception" && m.Genre == "Sci-Fi");
            
//             var persistedEvents = await context.MovieEvents.ToListAsync();
//             Assert.Contains(persistedEvents, e => e.MovieId == movieId1);

//             context.Dispose();
//             using var queryContext = CreateDbContext();
//             var queryMovieRepo = new MovieRepository(queryContext);
//             var queryEventRepo = new MovieEventRepository(queryContext);
//             var queryRoomRepo = new RoomRepository(queryContext); 
//             var queryUseCase = new FindMoviesWithEventsInTimeframeUseCase(queryMovieRepo, queryEventRepo, queryRoomRepo);

//             var query = new FindMoviesWithEventsInTimeframeQuery { Title = "Inception", Genre = "Sci-Fi" };
//             var result = await queryUseCase.ExecuteAsync(query);

//             Assert.NotEmpty(result);
//             Assert.Contains(result, m => m.Title == "Inception");
            
//             var inceptionResult = result.FirstOrDefault(m => m.Title == "Inception");
//             Assert.NotNull(inceptionResult);
//             Assert.Equal("Sci-Fi", inceptionResult.Genres);
//             Assert.NotEmpty(inceptionResult.Events);
//         }

//         [Fact]
//         public async Task ReturnsAllMoviesWithEventsInNext14Days_WhenNoFilter()
//         {
//             var context = CreateDbContext();
//             context.Database.EnsureCreated();
//             var movieRepo = new MovieRepository(context);
//             var roomRepo = new RoomRepository(context);
// context.MovieEvents.RemoveRange(context.MovieEvents);
// context.Movies.RemoveRange(context.Movies);

// await context.SaveChangesAsync();
// await roomRepo.SeedRoomsAsync();
//             var eventRepo = new MovieEventRepository(context);
//             var useCase = new FindMoviesWithEventsInTimeframeUseCase(movieRepo, eventRepo, roomRepo);

//             var movie1 = new Movie(Guid.NewGuid(), "Inception", "desc", "Sci-Fi", "actors", "12", 120, "poster");
//             var movie2 = new Movie(Guid.NewGuid(), "Titanic", "desc", "Drama", "actors", "12", 120, "poster");
//             await movieRepo.AddAsync(movie1);
//             await movieRepo.AddAsync(movie2);
//             var room = context.Rooms.First();

//             var movieEvent1 = new MovieEvent
//             {
//                 Id = Guid.NewGuid(),
//                 MovieId = movie1.Id,
//                 RoomId = room.Id,
//                 Date = DateTime.UtcNow.Date.AddDays(2),
//                 Time = new TimeSpan(15, 0, 0),
//                 Capacity = 50
//             };
//             var movieEvent2 = new MovieEvent
//             {
//                 Id = Guid.NewGuid(),
//                 MovieId = movie2.Id,
//                 RoomId = room.Id,
//                 Date = DateTime.UtcNow.Date.AddDays(3),
//                 Time = new TimeSpan(15, 0, 0),
//                 Capacity = 50
//             };
//             await eventRepo.AddAsync(movieEvent1);
//             await eventRepo.AddAsync(movieEvent2);

//             // Act
//             var query = new FindMoviesWithEventsInTimeframeQuery { Title = null, Genre = null };
//             var result = await useCase.ExecuteAsync(query);

//             // Assert
//             Assert.Equal(2, result.Count);
//             Assert.Contains(result, m => m.Title == "Inception");
//             Assert.Contains(result, m => m.Title == "Titanic");
//         }

//         [Fact]
//         public async Task ReturnsEmptyList_WhenNoEventsInNext14Days()
//         {
//             var context = CreateDbContext();
//             context.Database.EnsureCreated();
//             var movieRepo = new MovieRepository(context);
//             var roomRepo = new RoomRepository(context);
// context.MovieEvents.RemoveRange(context.MovieEvents);
// context.Movies.RemoveRange(context.Movies);
// await context.SaveChangesAsync();
// await roomRepo.SeedRoomsAsync();
//             var eventRepo = new MovieEventRepository(context);
//             var useCase = new FindMoviesWithEventsInTimeframeUseCase(movieRepo, eventRepo, roomRepo);

//             var movie = new Movie(Guid.NewGuid(), "Inception", "desc", "Sci-Fi", "actors", "12", 120, "poster");
//             await movieRepo.AddAsync(movie);
//             var room = context.Rooms.First();

//             var movieEvent = new MovieEvent
//             {
//                 Id = Guid.NewGuid(),
//                 MovieId = movie.Id,
//                 RoomId = room.Id,
//                 Date = DateTime.UtcNow.Date.AddDays(20),
//                 Time = new TimeSpan(15, 0, 0),
//                 Capacity = 50
//             };
//             await eventRepo.AddAsync(movieEvent);

//             // Act
//             var query = new FindMoviesWithEventsInTimeframeQuery { Title = "Inception", Genre = "Sci-Fi" };
//             var result = await useCase.ExecuteAsync(query);

//             // Assert
//             Assert.Empty(result);
//         }
//     }
// }
