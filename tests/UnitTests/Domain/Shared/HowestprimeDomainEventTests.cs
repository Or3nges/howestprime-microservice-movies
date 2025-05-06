using System;
using Howestprime.Movies.Domain.Shared;
using Xunit;

namespace UnitTests.Domain.Shared
{
    public class HowestprimeDomainEventTests
    {
        private class TestDomainEvent : HowestprimeDomainEvent
        {
            public TestDomainEvent(Guid id, string eventName) : base(eventName)
            {
                Id = id;
            }
            public Guid Id { get; }
        }

        [Fact]
        public void CanInstantiateConcreteDomainEvent()
        {
            var id = Guid.NewGuid();
            var evt = new TestDomainEvent(id, "TestEvent");
            Assert.Equal(id, evt.Id);
        }
    }
}
