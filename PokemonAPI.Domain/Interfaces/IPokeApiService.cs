using PokemonAPI.Domain.Models;

namespace PokemonAPI.Domain.Interfaces
{
    public interface IPokeApiService
    {
        public Task<PaginatedResponse<Pokemon>> GetPokemonsAsync(int limit = 20, int offset = 0, bool fetchDetails = true);
        public Task<IEnumerable<Pokemon>> GetRandomPokemonsAsync(int count = 10);
        public Task<Pokemon> GetPokemonByIdAsync(int id);
        public Task CapturePokemonAsync(CapturedPokemon capturedPokemon);
        public Task<IEnumerable<CapturedPokemon>> ListCapturedPokemonsAsync();
    }
}
