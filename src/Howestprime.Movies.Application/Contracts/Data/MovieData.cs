using System.Collections.Generic;
using System.Text.Json.Serialization;
using Howestprime.Movies.Application.Contracts.Data;

namespace Howestprime.Movies.Application.Contracts.Data
{
    public class MovieData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        [JsonPropertyName("genre")]
        public string Genre { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("year")]
        public int Year { get; set; }
        
        [JsonPropertyName("duration")]
        public int Duration { get; set; }
        
        [JsonPropertyName("actors")]
        public string Actors { get; set; }
        
        [JsonPropertyName("ageRating")]
        public int AgeRating { get; set; }
        
        [JsonPropertyName("posterUrl")]
        public string PosterUrl { get; set; }
        
        [JsonPropertyName("events")]
        public List<MovieEventData> Events { get; set; } = new();
    }
}