using PokemonAPI.Domain.Models;

namespace PokemonAPI.Domain.Interfaces
{
    public interface IPokeApiService
    {
        public Task<PaginatedResponse<Pokemon>> GetPokemonsAsync(int limit = 20, int offset = 0);
    }
}
