using System.Text.Json.Serialization;
namespace PokemonAPI.Domain.Models.EvolutionChain
{

    public class Chain
    {
        [JsonPropertyName("evolution_details")]
        public List<EvolutionDetail> EvolutionDetails { get; set; }

        [JsonPropertyName("evolves_to")]
        public List<EvolvesTo> EvolvesTo { get; set; }

        [JsonPropertyName("species")]
        public Species Species { get; set; }

        [JsonPropertyName("is_baby")]
        public bool IsBaby { get; set; }
    }

}