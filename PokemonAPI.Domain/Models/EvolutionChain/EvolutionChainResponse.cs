using System.Text.Json.Serialization;
using Newtonsoft.Json; 
namespace PokemonAPI.Domain.Models.EvolutionChain{ 

    public class EvolutionChainResponse
    {
        [JsonPropertyName("chain")]
        public Chain Chain { get; set; }
    }

}