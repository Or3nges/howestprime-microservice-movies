using System;
using Howestprime.Movies.Application.Movies.FindMovieById;
using Xunit;

namespace UnitTests.Application
{
    public class MovieByIdQueryTests
    {
        [Fact]
        public void MovieByIdQuery_CanBeCreated_WithValidData()
        {
            var id = Guid.NewGuid();
            var query = new MovieByIdQuery { Id = id };
            Assert.Equal(id, query.Id);
        }
    }
}
