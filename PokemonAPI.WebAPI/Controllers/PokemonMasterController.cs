using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PokemonAPI.Domain.DTOs;
using PokemonAPI.Domain.Interfaces;
using PokemonAPI.Domain.Models;

namespace PokemonAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonMasterController : ControllerBase
    {
        private readonly IPokemonMasterService _pokemonMasterService;

        public PokemonMasterController(IPokemonMasterService pokemonMasterService)
        {
            _pokemonMasterService = pokemonMasterService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPokemonMaster([FromBody] AddPokemonMasterRequest request)
        {
            var master = new PokemonMaster()
            {
                Age = request.Age,
                Name = request.Name,
                Cpf = request.Cpf
            };

            await _pokemonMasterService.AddPokemonMasterAsync(master);
            return Ok(new
            {
                Message = "Pokemon Master registered successfully.",
                PokemonMaster = master
            });
        }
    }
}