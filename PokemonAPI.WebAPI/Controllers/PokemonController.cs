using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PokemonAPI.Domain.DTOs;
using PokemonAPI.Domain.Exceptions;
using PokemonAPI.Domain.Interfaces;
using PokemonAPI.Domain.Models;
using PokemonAPI.Domain.Validators;

namespace PokemonAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokeApiService _pokeApiService;
        private readonly PokemonRequestValidator _validator;

        public PokemonController(IPokeApiService pokeApiService, PokemonRequestValidator validatior)
        {
            _pokeApiService = pokeApiService;
            _validator = validatior;
        }

        /// <summary>
        /// Obtém uma lista paginada de Pokémons.
        /// </summary>
        /// <param name="request">Parâmetros de requisição para paginação.</param>
        /// <returns>Uma lista paginada de Pokémons.</returns>
        [HttpGet]
        public async Task<IActionResult> GetPokemons([FromQuery] PokemonRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var pokemons = await _pokeApiService.GetPokemonsAsync(request.Limit, request.Offset);
            return Ok(new PaginatedResponse<PokemonDto>
            {
                Data = pokemons.Data.ToArray().Select(p => new PokemonDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Url = p.Url,
                    Evolutions = p.Evolutions,
                    Sprites = p.Sprites,
                    SpriteBase64 = p.SpriteBase64
                }),
                TotalCount = pokemons.TotalCount,
                PageSize = pokemons.PageSize,
                CurrentPage = pokemons.CurrentPage,
                NextPage = pokemons.NextPage,
                PreviousPage = pokemons.PreviousPage
            });
        }

        /// <summary>
        /// Obtém uma lista de Pokémons aleatórios.
        /// </summary>
        /// <returns>Uma lista de Pokémons aleatórios.</returns>
        [HttpGet("random")]
        public async Task<IActionResult> GetRandomPokemons()
        {
            var randomPokemons = await _pokeApiService.GetRandomPokemonsAsync();
            var pokemons = randomPokemons.Where(p => p != null).Select(p => new PokemonDto
            {
                Id = p.Id,
                Name = p.Name,
                Url = p.Url,
                Evolutions = p.Evolutions,
                Sprites = p.Sprites,
                SpriteBase64 = p.SpriteBase64
            });

            return Ok(pokemons);
        }

        /// <summary>
        /// Obtém detalhes de um Pokémon específico pelo seu ID.
        /// </summary>
        /// <param name="id">ID do Pokémon.</param>
        /// <returns>Detalhes do Pokémon.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPokemonById(int id)
        {
            var pokemon = await _pokeApiService.GetPokemonByIdAsync(id);
            if (pokemon == null)
            {
                return NotFound();
            }

            return Ok(new PokemonDto()
            {
                Evolutions = pokemon.Evolutions,
                Id = pokemon.Id,
                Name = pokemon.Name,
                Sprites = pokemon.Sprites,
                SpriteBase64 = pokemon.SpriteBase64,
                Url = pokemon.Url
            });
        }

        /// <summary>
        /// Captura um Pokémon específico pelo seu ID.
        /// </summary>
        /// <param name="pokemonId">ID do Pokémon a ser capturado.</param>
        /// <returns>Mensagem de sucesso e detalhes do Pokémon capturado.</returns>
        [HttpPost("capture/{pokemonId}")]
        public async Task<IActionResult> CapturePokemon(int pokemonId)
        {
            try
            {
                if (pokemonId <= 0)
                {
                    return base.BadRequest("Invalid Pokémon ID.");
                }

                var pokemon = await _pokeApiService.GetPokemonByIdAsync(pokemonId);
                if (pokemon == null)
                {
                    return base.BadRequest("Invalid Pokémon ID.");
                }

                var capturedPokemon = new CapturedPokemon
                {
                    PokemonId = pokemonId
                };

                await _pokeApiService.CapturePokemonAsync(capturedPokemon);

                return Ok(new
                {
                    Message = "Pokemon has captured successfully.",
                    CapturedPokemon = capturedPokemon
                });
            }
            catch (CapturedPokemonExistsException ex)
            {
                return base.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return base.StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Lista todos os Pokémons capturados.
        /// </summary>
        /// <returns>Uma lista de Pokémons capturados.</returns>
        [HttpGet("listCaptured")]
        public async Task<IActionResult> GetCapturedPokemons()
        {
            var capturedPokemons = await _pokeApiService.ListCapturedPokemonsAsync();
            if (capturedPokemons == null || !capturedPokemons.Any())
            {
                return NotFound("No captured Pokémon found.");
            }

            var pokemonDetails = new List<PokemonDto>();
            foreach (var capturedPokemon in capturedPokemons)
            {
                var pokemon = await _pokeApiService.GetPokemonByIdAsync(capturedPokemon.PokemonId);
                if (pokemon != null)
                {
                    pokemonDetails.Add(new PokemonDto
                    {
                        Id = pokemon.Id,
                        Name = pokemon.Name,
                        Url = pokemon.Url,
                        Evolutions = pokemon.Evolutions,
                        Sprites = pokemon.Sprites,
                        SpriteBase64 = pokemon.SpriteBase64
                    });
                }
            }

            return Ok(pokemonDetails);
        }
    }
}
