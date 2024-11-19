using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PokemonAPI.Domain.DTOs;
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
                    Url = p.Url
                }),
                TotalCount = pokemons.TotalCount,
                PageSize = pokemons.PageSize,
                CurrentPage = pokemons.CurrentPage,
                NextPage = pokemons.NextPage,
                PreviousPage = pokemons.PreviousPage
            });

        }
    }
}
