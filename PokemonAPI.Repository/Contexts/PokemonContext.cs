using Microsoft.EntityFrameworkCore;
using PokemonAPI.Domain.Models;

namespace PokemonAPI.Repository.Contexts
{
    public class PokemonContext : DbContext
    {
        public DbSet<CapturedPokemon> CapturedPokemons { get; set; }
        public DbSet<PokemonMaster> PokemonMasters { get; set; }

        public PokemonContext(DbContextOptions<PokemonContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=../PokemonAPI.Repository/pokemon.db",
                    b => b.MigrationsAssembly("PokemonAPI.Repository"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CapturedPokemon>()
                .ToTable("CapturedPokemons")
                .HasKey(cp => new { cp.Id });
            modelBuilder.Entity<PokemonMaster>()
                .ToTable("PokemonMasters")
                .HasKey(pm => pm.Id);
        }
    }
}
