using System;
using Howestprime.Movies.Domain.Shared.Exceptions;
using Xunit;

namespace UnitTests.Domain.Shared
{
    public class NotFoundExceptionTests
    {
        [Fact]
        public void Constructor_WithMessage_CreatesException()
        {
            var message = "Test not found";
            var exception = new NotFoundException(message);

            Assert.Equal(message, exception.Message);
        }
    }
} 