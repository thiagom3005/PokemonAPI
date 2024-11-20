using System.Text.Json.Serialization;

namespace PokemonAPI.Domain.Models.Sprite
{

    public class GenerationViii
    {
        [JsonPropertyName("icons")]
        public Icons Icons { get; set; }
    }

}