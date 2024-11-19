using System.Net;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using PokemonAPI.Domain.Interfaces;
using PokemonAPI.Service;

namespace PokemonAPI.Tests.Services
{
    public class PokeApiServiceTests
    {
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly Mock<ILogger<IPokeApiService>> _mockLogger;
        private readonly IPokeApiService _service;

        public PokeApiServiceTests()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockMemoryCache = new Mock<IMemoryCache>();
            _mockLogger = new Mock<ILogger<IPokeApiService>>();

            var httpClient = new HttpClient(new FakeHttpMessageHandler(HttpStatusCode.OK, MockPokeApiResponses.GetPokemonListResponse()))
            {
                BaseAddress = new Uri("https://pokeapi.co/api/v2/")
            };

            _mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _service = new PokeApiService(httpClient, _mockMemoryCache.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetPokemonsAsync_ReturnsPaginatedResponse_WithListOfPokemons()
        {
            // Arrange
            var responseContent = MockPokeApiResponses.GetPokemonListResponse();
            var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, responseContent);
            var httpClient = new HttpClient(fakeHandler);

            _mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _service.GetPokemonsAsync(20, 0);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Data.Count());
            Assert.Equal("Pikachu", result.Data.First().Name);
        }

        [Fact]
        public async Task GetPokemonsAsync_ReturnsEmptyPaginatedResponse_WhenNoPokemonsFound()
        {
            // Arrange
            var responseContent = "[]";
            var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.OK, responseContent);
            var httpClient = new HttpClient(fakeHandler);

            _mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _service.GetPokemonsAsync(20, 0);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetPokemonsAsync_ThrowsException_WhenInternalServerError()
        {
            // Arrange
            var fakeHandler = new FakeHttpMessageHandler(HttpStatusCode.InternalServerError, "Internal Server Error");
            var httpClient = new HttpClient(fakeHandler);

            _mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _service.GetPokemonsAsync(20, 0));
        }
    }
}


