using PokemonAPI.Domain.Models;

namespace PokemonAPI.Domain.Interfaces
{
    public interface IPokemonMasterService
    {
        Task AddPokemonMasterAsync(PokemonMaster master);
    }
}
