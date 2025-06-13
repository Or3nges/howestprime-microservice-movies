using System;
using Domaincrafters.Domain;

namespace Howestprime.Movies.Domain.Entities
{
    public class Room : Entity<RoomId>
    {
        public string Name { get; private set; }
        public int Capacity { get; private set; }

        public Room(RoomId id, string name, int capacity) : base(id)
        {
            Name = name;
            Capacity = capacity;
        }

        public override void ValidateState()
        {

        }
    }

    public sealed class RoomId : UuidEntityId
    {
        public RoomId(string? id = "") : base(id)
        {
        }
    }
}
