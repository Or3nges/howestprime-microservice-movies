using Howestprime.Movies.Domain.Shared;
using Xunit;
using Domaincrafters.Domain;

namespace UnitTests.Domain.Shared
{
    public class DomainEventTests
    {
        private class TestDomainEvent : HowestprimeDomainEvent
        {
            public TestDomainEvent() : base("test.event")
            {
            }
        }

        [Fact]
        public void HowestprimeDomainEvent_CanBeCreated()
        {
            var domainEvent = new TestDomainEvent();
            
            Assert.Equal("test.event", domainEvent.EventName);
        }
    }
} 