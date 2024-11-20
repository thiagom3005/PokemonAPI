using System.Text.Json.Serialization;

namespace PokemonAPI.Domain.Models.Sprite
{

    public class GenerationV
    {
        [JsonPropertyName("black-white")]
        public BlackWhite BlackWhite { get; set; }
    }

}