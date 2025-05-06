using System;
using Howestprime.Movies.Domain.Shared.Exceptions;
using Xunit;

namespace UnitTests.Domain.Shared.Exceptions
{
    public class NotFoundExceptionTests
    {
        [Fact]
        public void NotFoundException_ThrowsWithMessage()
        {
            var ex = new NotFoundException("Test message");
            Assert.Equal("Test message", ex.Message);
        }
    }
}
