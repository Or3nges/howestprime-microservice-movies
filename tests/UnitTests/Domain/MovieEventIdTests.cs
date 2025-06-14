using Howestprime.Movies.Domain.MovieEvent;
using Xunit;

namespace UnitTests.Domain
{
    public class MovieEventIdTests
    {
        [Fact]
        public void CreateUnique_Returns_New_MovieEventId()
        {
            var movieEventId = MovieEventId.CreateUnique();
            Assert.NotNull(movieEventId);
        }
    }
} 