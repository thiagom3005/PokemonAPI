using PokemonAPI.Domain.Interfaces;
using PokemonAPI.Domain.Models;
using PokemonAPI.Repository.Contexts;

namespace PokemonAPI.Service
{
    public class PokemonMasterService : IPokemonMasterService
    {
        private readonly PokemonContext _context;

        public PokemonMasterService(PokemonContext context)
        {
            _context = context;
        }

        public async Task AddPokemonMasterAsync(PokemonMaster master)
        {
            _context.PokemonMasters.Add(master);
            await _context.SaveChangesAsync();
        }
    }
}
