using System.Text.Json.Serialization;
namespace PokemonAPI.Domain.Models.EvolutionChain
{

    public class EvolvesTo
    {
        [JsonPropertyName("evolution_details")]
        public List<EvolutionDetail> EvolutionDetails { get; set; }

        [JsonPropertyName("evolves_to")]
        public List<EvolvesTo> EvolvesAfter { get; set; }

        [JsonPropertyName("species")]
        public Species Species { get; set; }

        [JsonPropertyName("is_baby")]
        public bool IsBaby { get; set; }
    }

}