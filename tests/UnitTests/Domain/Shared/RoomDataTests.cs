using System;
using Xunit;
using Howestprime.Movies.Domain.Shared;
using Howestprime.Movies.Domain.Entities;

namespace UnitTests.Domain.Shared
{
    public class RoomDataTests
    {
        [Fact]
        public void RoomData_CanBeCreated_WithValidData()
        {
            var id = new RoomId();
            var roomData = new RoomData
            {
                Id = id,
                Name = "Room 1",
                Capacity = 50
            };
            Assert.Equal(id, roomData.Id);
            Assert.Equal("Room 1", roomData.Name);
            Assert.Equal(50, roomData.Capacity);
        }

        [Fact]
        public void RoomData_DefaultValues_AreCorrect()
        {
            var roomData = new RoomData();
            Assert.Equal(new RoomId(), roomData.Id);
            Assert.Null(roomData.Name);
            Assert.Equal(0, roomData.Capacity);
        }
    }
}
