using System;
using System.Text.Json.Serialization;

namespace Howestprime.Movies.Domain.Shared
{
    public class MovieEventData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("room")]
        public RoomData Room { get; set; }
        
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
        
        [JsonPropertyName("movieId")]
        public string MovieId { get; set; }
        
        [JsonPropertyName("capacity")]
        public int Capacity { get; set; }
    }
}