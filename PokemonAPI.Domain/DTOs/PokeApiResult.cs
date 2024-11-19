using Newtonsoft.Json;

namespace PokemonAPI.Domain.DTOs
{
    public class PokeApiResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("previous")]
        public string Previous { get; set; }

        [JsonProperty("results")]
        public List<PokeApiResult> Results { get; set; }
    }
    public class PokeApiResult
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
