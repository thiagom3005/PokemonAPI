using System.Text.Json.Serialization;

namespace PokemonAPI.Domain.Models.Sprite
{

    public class Gold
    {
        [JsonPropertyName("back_default")]
        public string BackDefault { get; set; }

        [JsonPropertyName("back_shiny")]
        public string BackShiny { get; set; }

        [JsonPropertyName("front_default")]
        public string FrontDefault { get; set; }

        [JsonPropertyName("front_shiny")]
        public string FrontShiny { get; set; }

        [JsonPropertyName("front_transparent")]
        public string FrontTransparent { get; set; }
    }

}