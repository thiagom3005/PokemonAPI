using System.Text.Json.Serialization;
namespace PokemonAPI.Domain.Models.EvolutionChain
{

    public class Trigger
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

}