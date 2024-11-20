using System.Text.Json.Serialization;

namespace PokemonAPI.Domain.Models.Sprite
{

    public class OmegarubyAlphasapphire
    {
        [JsonPropertyName("front_default")]
        public string FrontDefault { get; set; }

        [JsonPropertyName("front_female")]
        public string FrontFemale { get; set; }

        [JsonPropertyName("front_shiny")]
        public string FrontShiny { get; set; }

        [JsonPropertyName("front_shiny_female")]
        public string FrontShinyFemale { get; set; }
    }

}