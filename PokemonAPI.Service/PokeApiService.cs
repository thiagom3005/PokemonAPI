using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PokemonAPI.Domain.DTOs;
using PokemonAPI.Domain.Exceptions;
using PokemonAPI.Domain.Interfaces;
using PokemonAPI.Domain.Models;
using PokemonAPI.Domain.Models.EvolutionChain;
using PokemonAPI.Repository.Contexts;

namespace PokemonAPI.Service
{
    public class PokeApiService : IPokeApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<IPokeApiService> _logger;
        private readonly PokemonContext _context;
        private const string CacheKey = "PokemonList";

        public PokeApiService(HttpClient httpClient, IMemoryCache memoryCache, ILogger<IPokeApiService> logger, PokemonContext context)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
            _logger = logger;
            _context = context;
        }
        public async Task<PaginatedResponse<Pokemon>> GetPokemonsAsync(int limit = 20, int offset = 0, bool fetchDetails = true)
        {
            try
            {
                _logger.LogInformation("Fetching Pokémon list: limit={Limit}, offset={Offset}", limit, offset);
                var cacheKey = $"{CacheKey}_limit_{limit}_offset_{offset}";

                if (_memoryCache.TryGetValue(cacheKey, out PaginatedResponse<Pokemon> cachedPokemons))
                {
                    _logger.LogWarning("No results found from PokeAPI.");
                    return cachedPokemons;
                }

                var response = await _httpClient.GetAsync($"pokemon?limit={limit}&offset={offset}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<PokeApiResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null || result.Results == null)
                {
                    return new PaginatedResponse<Pokemon>
                    {
                        Data = new List<Pokemon>(),
                        TotalCount = 0,
                        PageSize = limit,
                        CurrentPage = (offset / limit) + 1,
                        NextPage = null,
                        PreviousPage = null
                    };
                }

                var pokemons = result.Results.Select((pokemon, index) => new Pokemon
                {
                    Id = offset + index + 1,
                    Name = pokemon.Name,
                    Url = pokemon.Url
                }).ToList();

                if (fetchDetails)
                {
                    foreach (var pokemon in pokemons)
                    {
                        var pokemonDetailed = await GetPokemonDetailsAsync(pokemon);
                        if (pokemonDetailed != null)
                        {
                            pokemon.Sprites = pokemonDetailed.Sprites;
                            pokemon.Evolutions = pokemonDetailed.Evolutions;
                            pokemon.SpriteBase64 = pokemonDetailed.SpriteBase64;
                        }
                    }
                }

                var paginatedPokemons = new PaginatedResponse<Pokemon>
                {
                    Data = pokemons,
                    TotalCount = result.Count,
                    PageSize = limit,
                    CurrentPage = (offset / limit) + 1,
                    NextPage = result.Next,
                    PreviousPage = result.Previous
                };

                _memoryCache.Set(cacheKey, paginatedPokemons, TimeSpan.FromMinutes(5));

                return paginatedPokemons;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Pokémon list.");
                throw;
            }
        }

        public async Task<IEnumerable<Pokemon>> GetRandomPokemonsAsync(int count = 10)
        {
            var random = new Random();
            var allPokemons = await GetPokemonsAsync(1, 0, false);
            allPokemons = await GetPokemonsAsync(allPokemons.TotalCount, 0, false);
            var randomPokemons = allPokemons.Data.OrderBy(x => random.Next()).Take(count).ToArray();

            if (randomPokemons == null)
            {
                _logger.LogWarning("No random Pokémon found.");
                return new List<Pokemon>();
            }

            foreach (var pokemon in randomPokemons)
            {
                var pokemonDetailed = await GetPokemonDetailsAsync(pokemon);
                if (pokemonDetailed != null)
                {
                    pokemon.Sprites = pokemonDetailed.Sprites;
                    pokemon.Evolutions = pokemonDetailed.Evolutions;
                    pokemon.SpriteBase64 = pokemonDetailed.SpriteBase64;
                }
            }

            return randomPokemons;
        }

        public async Task<Pokemon> GetPokemonByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching Pokémon with ID: {Id}", id);

                var response = await _httpClient.GetAsync($"pokemon/{id}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var pokemon = JsonSerializer.Deserialize<Pokemon>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (pokemon == null)
                {
                    _logger.LogWarning("No Pokémon found with ID: {Id}", id);
                    return null;
                }

                var pokemonDetailed = await GetPokemonDetailsAsync(pokemon);
                if (pokemonDetailed != null)
                {
                    pokemon.Sprites = pokemonDetailed.Sprites;
                    pokemon.Evolutions = pokemonDetailed.Evolutions;
                    pokemon.SpriteBase64 = pokemonDetailed.SpriteBase64;
                }

                return pokemon;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Pokémon with ID: {Id}", id);
                throw;
            }
        }

        public async Task<Pokemon> GetPokemonDetailsAsync(Pokemon pokemon)
        {
            if (pokemon.Sprites == null)
            {
                var response = await _httpClient.GetAsync($"pokemon/{pokemon.Id}");
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    return pokemon;

                var content = await response.Content.ReadAsStringAsync();
                pokemon = JsonSerializer.Deserialize<Pokemon>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            var evolutionResponse = await _httpClient.GetAsync($"evolution-chain/{pokemon.Id}");
            if (evolutionResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var evolutionContent = await evolutionResponse.Content.ReadAsStringAsync();
                var evolutionChain = JsonSerializer.Deserialize<EvolutionChainResponse>(evolutionContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                pokemon.Evolutions = evolutionChain.Chain.EvolvesTo?.ToList();
            }

            var spriteResponse = await _httpClient.GetAsync(pokemon.Sprites.FrontDefault);
            if (spriteResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var spriteBytes = await spriteResponse.Content.ReadAsByteArrayAsync();
                pokemon.SpriteBase64 = Convert.ToBase64String(spriteBytes);
            }

            return pokemon;
        }

        public async Task CapturePokemonAsync(CapturedPokemon capturedPokemon)
        {
            var capturedPokemonExists = await _context.CapturedPokemons.AnyAsync(x => x.PokemonId == capturedPokemon.PokemonId);
            if (capturedPokemonExists)
            {
                _logger.LogWarning("Pokemon with ID {PokemonId} has already been captured.", capturedPokemon.PokemonId);
                throw new CapturedPokemonExistsException();
            }

            _context.CapturedPokemons.Add(capturedPokemon);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CapturedPokemon>> ListCapturedPokemonsAsync()
        {
            return await _context.CapturedPokemons.ToListAsync();
        }
    }
}