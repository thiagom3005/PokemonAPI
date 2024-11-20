using System.Text.Json.Serialization;

namespace PokemonAPI.Domain.Models.Sprite
{

    public class GenerationI
    {
        [JsonPropertyName("red-blue")]
        public RedBlue RedBlue { get; set; }

        [JsonPropertyName("yellow")]
        public Yellow Yellow { get; set; }
    }

}