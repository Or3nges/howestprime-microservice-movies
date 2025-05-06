using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Application
{
    public class SuspendUserCommandHandlerTests
    {
        [Fact]
        public async Task SuspendUserCommandHandler_DummyTest()
        {
            await Task.CompletedTask;
            Assert.True(true);
        }
    }
}
