using System.Text.Json.Serialization;
using PokemonAPI.Domain.Models.EvolutionChain;
using PokemonAPI.Domain.Models.Sprite;

namespace PokemonAPI.Domain.Models
{
    public class Pokemon
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("sprites")]
        public Sprites Sprites { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        public List<EvolvesTo> Evolutions { get; set; }

        public string SpriteBase64 { get; set; }
    }
}
