// using System;
// using System.Threading.Tasks;
// using Howestprime.Movies.Application;
// using Howestprime.Movies.Application.Contracts.Ports;
// using Howestprime.Movies.Application.Movies.RegisterMovie;
// using Howestprime.Movies.Domain.Entities;
// using Howestprime.Movies.Infrastructure.Persistence;
// using Xunit;

// namespace UnitTests.IntegrationTests
// {
//     public class RegisterMovieTests
//     {
//         [Fact]
//         public async Task RegisterMovie_SuccessfullyPersistsMovie()
//         {
//             // Arrange
//             IMovieRepository movieRepository = new MovieRepository();
//             var useCase = new RegisterMovieUseCase(movieRepository);
//             var command = new RegisterMovieCommand
//             {
//                 Title = "Inception",
//                 Description = "A mind-bending thriller",
//                 Genre = "Sci-Fi",
//                 Actors = "Leonardo DiCaprio",
//                 AgeRating = 12,
//                 Duration = 148,
//                 PosterUrl = "https://example.com/inception.jpg"
//             };

//             // Act
//             Movie movie = await useCase.ExecuteAsync(command);

//             // Assert
//             Assert.NotNull(movie);
//             Assert.Equal("Inception", movie.Title);
//             Assert.Equal("A mind-bending thriller", movie.Description);
//             Assert.Equal("Sci-Fi", movie.Genre);
//             Assert.Equal("Leonardo DiCaprio", movie.Actors);
//             Assert.Equal("12", movie.AgeRating);
//             Assert.Equal(148, movie.Duration);
//             Assert.Equal("https://example.com/inception.jpg", movie.PosterUrl);
//         }
//     }
// }
