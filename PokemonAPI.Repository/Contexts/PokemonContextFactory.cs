using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PokemonAPI.Repository.Contexts
{
    public class PokemonContextFactory : IDesignTimeDbContextFactory<PokemonContext>
    {
        public PokemonContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PokemonContext>();
            optionsBuilder.UseSqlite("Data Source=../PokemonAPI.Repository/pokemon.db",
                b => b.MigrationsAssembly("PokemonAPI.Repository"));

            return new PokemonContext(optionsBuilder.Options);
        }
    }
}
