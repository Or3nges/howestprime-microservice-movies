// using System;
// using System.Threading.Tasks;
// using Howestprime.Movies.Application.Contracts.Ports;
// using Howestprime.Movies.Application.Movies.FindMovieById;
// using Howestprime.Movies.Domain.Entities;
// using Howestprime.Movies.Domain.Shared;
// using Howestprime.Movies.Infrastructure.Persistence.EntityFramework;
// using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Repositories;
// using Microsoft.EntityFrameworkCore;
// using Xunit;

// namespace UnitTests.IntegrationTests
// {
//     public class FindMovieByIdTests
//     {
//         private MoviesDbContext CreateDbContext()
//         {
//             var options = new DbContextOptionsBuilder<MoviesDbContext>()
//                 .UseNpgsql("Host=localhost;Port=40021;Database=movies-howestprime;Username=devuser;Password=devpassword")
//                 .Options;
//             return new MoviesDbContext(options);
//         }

//         [Fact]
//         public async Task FindMovieById_ReturnsCorrectMovie()
//         {
//             // Arrange
//             var context = CreateDbContext();
//             context.Database.EnsureCreated();
//             var repo = new MovieRepository(context);
//             var useCase = new FindMovieByIdUseCase(repo);
//             var movie = new Howestprime.Movies.Domain.Entities.Movie(Guid.NewGuid(), "FindByIdTest", "desc", "genre", "actors", "12", 120, "poster");
//             await repo.AddAsync(movie);

//             // Act
//             var query = new MovieByIdQuery { Id = movie.Id };
//             MovieData result = await useCase.ExecuteAsync(query);

//             // Assert
//             Assert.Equal(movie.Id, result.Id);
//             Assert.Equal("FindByIdTest", result.Title);
//         }

//         [Fact]
//         public async Task FindMovieById_NotFound_Throws()
//         {
//             // Arrange
//             var context = CreateDbContext();
//             var repo = new MovieRepository(context);
//             var useCase = new FindMovieByIdUseCase(repo);
//             var query = new MovieByIdQuery { Id = Guid.NewGuid() };

//             // Act & Assert
//             await Assert.ThrowsAsync<Exception>(() => useCase.ExecuteAsync(query));
//         }
//     }
// }
