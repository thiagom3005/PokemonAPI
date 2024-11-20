using System.Text.Json.Serialization;
namespace PokemonAPI.Domain.Models.EvolutionChain
{

    public class Species
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

}