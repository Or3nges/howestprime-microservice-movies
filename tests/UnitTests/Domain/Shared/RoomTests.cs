using System;
using Howestprime.Movies.Domain.Shared;
using Xunit;

namespace UnitTests.Domain.Shared
{
    public class RoomTests
    {
        [Fact]
        public void Room_CanBeCreated_WithValidData()
        {
            var id = Guid.NewGuid();
            var room = new Room(id, "Room", 10);
            Assert.Equal(id, room.Id);
            Assert.Equal("Room", room.Name);
            Assert.Equal(10, room.Capacity);
        }

        [Fact]
        public void Room_DefaultValues_AreCorrect()
        {
            var room = new Room(Guid.Empty, string.Empty, 0);
            Assert.Equal(Guid.Empty, room.Id);
            Assert.Equal(string.Empty, room.Name);
            Assert.Equal(0, room.Capacity);
        }
    }
}
