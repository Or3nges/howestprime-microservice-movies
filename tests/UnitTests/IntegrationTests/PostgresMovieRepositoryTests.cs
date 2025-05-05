// using System;
// using System.Threading.Tasks;
// using Howestprime.Movies.Domain.Entities;
// using Howestprime.Movies.Infrastructure.Persistence.EntityFramework;
// using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Repositories;
// using Microsoft.EntityFrameworkCore;
// using Xunit;

// namespace UnitTests.IntegrationTests
// {
//     public class PostgresMovieRepositoryTests
//     {
//         private MoviesDbContext CreateDbContext()
//         {
//             var options = new DbContextOptionsBuilder<MoviesDbContext>()
//                 .UseNpgsql("Host=localhost;Port=40021;Database=movies-howestprime;Username=devuser;Password=devpassword")
//                 .Options;
//             return new MoviesDbContext(options);
//         }

//         [Fact]
//         public async Task AddAndGetMovie_WorksWithPostgres()
//         {
//             // Arrange
//             var context = CreateDbContext();
//             context.Database.EnsureCreated();
//             var repo = new MovieRepository(context);
//             var movie = new Movie(Guid.NewGuid(), "PostgresTest", "desc", "genre", "actors", "12", 120, "poster");

//             // Act
//             await repo.AddAsync(movie);
//             var fetched = await repo.GetByIdAsync(movie.Id);

//             // Assert
//             Assert.NotNull(fetched);
//             Assert.Equal("PostgresTest", fetched.Title);
//         }
//     }
// }