using System.Text.Json.Serialization;

namespace PokemonAPI.Domain.Models.Sprite
{

    public class GenerationIi
    {
        [JsonPropertyName("crystal")]
        public Crystal Crystal { get; set; }

        [JsonPropertyName("gold")]
        public Gold Gold { get; set; }

        [JsonPropertyName("silver")]
        public Silver Silver { get; set; }
    }

}