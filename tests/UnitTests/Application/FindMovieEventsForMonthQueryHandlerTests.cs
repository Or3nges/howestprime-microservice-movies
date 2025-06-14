using System.Collections.Generic;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Data;
using Howestprime.Movies.Application.Movies.FindMovieEventsForMonth;
using UnitTests.Shared;
using Xunit;

namespace UnitTests.Application
{
    public class FindMovieEventsForMonthQueryHandlerTests
    {
        [Fact]
        public async Task HandleAsync_ShouldCallUseCaseAndReturnData()
        {
            // Arrange
            var movieRepository = new FakeMovieRepository();
            var movieEventRepository = new FakeMovieEventRepository();
            var roomRepository = new FakeRoomRepository();
            
            var useCase = new FindMovieEventsForMonthUseCase(movieRepository, movieEventRepository, roomRepository);
            var handler = new FindMovieEventsForMonthQueryHandler(useCase);
            var query = new FindMovieEventsForMonthQuery { Year = 2024, Month = 10 };

            // Act
            var result = await handler.HandleAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<MovieEventData>>(result);
        }
    }
} 