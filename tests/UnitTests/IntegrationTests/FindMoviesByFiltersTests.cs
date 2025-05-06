// using System;
// using System.Linq;
// using System.Threading.Tasks;
// using Howestprime.Movies.Application.Contracts.Ports;
// using Howestprime.Movies.Application.Movies.FindMoviesByFilters;
// using Howestprime.Movies.Application.Movies.RegisterMovie;
// using Howestprime.Movies.Domain.Entities;
// using Howestprime.Movies.Infrastructure.Persistence;
// using Xunit;

// namespace UnitTests.IntegrationTests
// {
//     public class FindMoviesByFiltersTests
//     {
//         private readonly IMovieRepository _repo;
//         private readonly FindMoviesByFiltersUseCase _useCase;

//         public FindMoviesByFiltersTests()
//         {
//             _repo = new MovieRepository();
//             _useCase = new FindMoviesByFiltersUseCase(_repo);

//             _repo.AddAsync(new Movie(Guid.NewGuid(), "Inception", "A mind-bending thriller", "Sci-Fi", "Leonardo DiCaprio", "12", 148, "url1"));
//             _repo.AddAsync(new Movie(Guid.NewGuid(), "The Matrix", "Virtual reality action", "Sci-Fi", "Keanu Reeves", "16", 136, "url2"));
//             _repo.AddAsync(new Movie(Guid.NewGuid(), "Titanic", "Romantic drama", "Drama", "Leonardo DiCaprio", "12", 195, "url3"));
//         }

//         [Fact]
//         public async Task Filter_By_Title_And_Genre_Returns_Correct_Movie()
//         {
//             var query = new FindMoviesByFiltersQuery { Title = "Matrix", Genre = "Sci-Fi" };
//             var result = await _useCase.ExecuteAsync(query);
//             Assert.Single(result);
//             Assert.Equal("The Matrix", result.First().Title);
//         }

//         [Fact]
//         public async Task Filter_No_Match_Returns_Empty()
//         {
//             var query = new FindMoviesByFiltersQuery { Title = "Nonexistent", Genre = "Fantasy" };
//             var result = await _useCase.ExecuteAsync(query);
//             Assert.Empty(result);
//         }
//     }
// }
