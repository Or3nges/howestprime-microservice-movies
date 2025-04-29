namespace Howestprime.Movies.Domain.Shared
{
    public class Room
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int Capacity { get; private set; }

        public Room(Guid id, string name, int capacity)
        {
            Id = id;
            Name = name;
            Capacity = capacity;
        }
    }
}