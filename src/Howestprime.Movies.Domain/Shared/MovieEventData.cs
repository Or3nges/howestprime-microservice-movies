using System.Text.Json.Serialization;

namespace Howestprime.Movies.Domain.Shared
{
    public class MovieEventData
    {
        public Guid Id { get; set; }
        
        [JsonPropertyName("time")]
        public DateTime DateTime { get; set; }  // Combined date and time in UTC
        
        public RoomData Room { get; set; }
        public int Capacity { get; set; }
        public int Visitors { get; set; }
    }
}