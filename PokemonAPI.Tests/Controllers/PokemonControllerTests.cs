using System.Net;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PokemonAPI.Domain.DTOs;
using PokemonAPI.Domain.Interfaces;
using PokemonAPI.Domain.Models;
using PokemonAPI.Domain.Validators;
using PokemonAPI.WebAPI.Controllers;

namespace PokemonAPI.Tests.Controllers
{
    public class PokemonControllerTests
    {
        private readonly Mock<IPokeApiService> _mockPokemonService;
        private readonly PokemonController _controller;

        public PokemonControllerTests()
        {
            _mockPokemonService = new Mock<IPokeApiService>();
            _controller = new PokemonController(_mockPokemonService.Object, new PokemonRequestValidator());
        }

        [Fact]
        public async Task GetAllPokemons_ReturnsOkResult_WithListOfPokemons()
        {
            // Arrange
            var responseContent = MockPokeApiResponses.GetPokemonListResponse();
            var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, responseContent);
            var httpClient = new HttpClient(fakeHandler);

            _mockPokemonService.Setup(service => service.GetPokemonsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new PaginatedResponse<Pokemon>
            {
                Data = new List<Pokemon>
                {
                    new Pokemon { Id = 1, Name = "Pikachu" },
                    new Pokemon { Id = 2, Name = "Charmander" }
                },
                TotalCount = 2,
                PageSize = 20,
                CurrentPage = 1
            });

            // Act
            var result = await _controller.GetPokemons(new PokemonRequest { Limit = 20, Offset = 0 });

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PaginatedResponse<PokemonDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Data.Count());
        }

        [Fact]
        public async Task GetPokemons_ReturnsBadRequest_WhenRequestIsInvalid()
        {
            // Arrange
            var invalidRequest = new PokemonRequest { Limit = -1, Offset = -1 };

            // Act
            var result = await _controller.GetPokemons(invalidRequest);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetPokemons_ReturnsEmptyList_WhenNoPokemonsFound()
        {
            // Arrange
            _mockPokemonService.Setup(service => service.GetPokemonsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new PaginatedResponse<Pokemon>
            {
                Data = new List<Pokemon>(),
                TotalCount = 0,
                PageSize = 20,
                CurrentPage = 1
            });

            // Act
            var result = await _controller.GetPokemons(new PokemonRequest { Limit = 20, Offset = 0 });

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PaginatedResponse<PokemonDto>>(okResult.Value);
            Assert.Empty(returnValue.Data);
        }
    }
}
