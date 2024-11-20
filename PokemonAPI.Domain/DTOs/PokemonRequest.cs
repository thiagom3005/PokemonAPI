namespace PokemonAPI.Domain.DTOs
{
    public class PokemonRequest
    {
        public int Limit { get; set; } = 20;
        public int Offset { get; set; } = 0;
    }
}
