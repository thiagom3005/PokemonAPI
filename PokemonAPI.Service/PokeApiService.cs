using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PokemonAPI.Domain.DTOs;
using PokemonAPI.Domain.Interfaces;
using PokemonAPI.Domain.Models;

namespace PokemonAPI.Service
{
    public class PokeApiService : IPokeApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<IPokeApiService> _logger;
        private const string CacheKey = "PokemonList";

        public PokeApiService(HttpClient httpClient, IMemoryCache memoryCache, ILogger<IPokeApiService> logger)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
            _logger = logger;
        }
        public async Task<PaginatedResponse<Pokemon>> GetPokemonsAsync(int limit = 20, int offset = 0)
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
    }
}