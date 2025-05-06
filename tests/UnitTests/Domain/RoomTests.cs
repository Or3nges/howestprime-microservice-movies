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
            var id = Guid.NewGuid();
            var room = new Room { Id = id, Name = "Room 1", Capacity = 50 };
            Assert.Equal(id, room.Id);
            Assert.Equal("Room 1", room.Name);
            Assert.Equal(50, room.Capacity);
        }

        [Fact]
        public void Room_NegativeCapacity_AllowedByDefault()
        {
            var room = new Room { Id = Guid.NewGuid(), Name = "Test Room", Capacity = -10 };
            Assert.Equal(-10, room.Capacity);
        }

        [Fact]
        public void Room_DefaultValues_AreCorrect()
        {
            var room = new Room();
            Assert.Equal(Guid.Empty, room.Id);
            Assert.Null(room.Name);
            Assert.Equal(0, room.Capacity);
        }
    }
}
