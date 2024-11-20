using PokemonAPI.Domain.Models.EvolutionChain;
using PokemonAPI.Domain.Models.Sprite;

namespace PokemonAPI.Domain.DTOs
{
    public class PokemonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Sprites Sprites { get; set; }
        public List<EvolvesTo> Evolutions { get; set; }
        public string SpriteBase64 { get; set; }
    }
}
