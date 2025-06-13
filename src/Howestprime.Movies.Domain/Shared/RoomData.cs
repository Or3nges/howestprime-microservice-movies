using System.Text.Json.Serialization;
using Howestprime.Movies.Domain.Entities;

namespace Howestprime.Movies.Domain.Shared
{
    public class RoomData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("capacity")]
        public int Capacity { get; set; }
    }
}