using System;
using Howestprime.Movies.Domain.Entities;
using Xunit;

namespace UnitTests.Domain
{
    public class RoomTests
    {
        [Fact]
        public void Room_CanBeCreated_WithValidData()
        {
            var id = new RoomId();
            var room = new Room(id, "Room 1", 50);
            Assert.Equal(id, room.Id);
            Assert.Equal("Room 1", room.Name);
            Assert.Equal(50, room.Capacity);
        }

        [Fact]
        public void Room_NegativeCapacity_AllowedByDefault()
        {
            var room = new Room(new RoomId(), "Test Room", -10);
            Assert.Equal(-10, room.Capacity);
        }

        [Fact]
        public void Room_DefaultValues_AreCorrect()
        {
            var room = new Room(new RoomId(), "Default", 0);
            Assert.NotNull(room.Id);
            Assert.Equal("Default", room.Name);
            Assert.Equal(0, room.Capacity);
        }
    }
}
