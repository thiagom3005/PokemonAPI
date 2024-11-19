public static class MockPokeApiResponses
{
    public static string GetPokemonListResponse() =>
        @"{
            ""count"": 1302,
            ""next"": ""https://pokeapi.co/api/v2/pokemon?offset=20&limit=20"",
            ""previous"": null,
            ""results"": [
                { ""name"": ""bulbasaur"", ""url"": ""https://pokeapi.co/api/v2/pokemon/1/"" },
                { ""name"": ""ivysaur"", ""url"": ""https://pokeapi.co/api/v2/pokemon/2/"" }
            ]
        }";

    public static string GetEmptyResponse() =>
        @"{
            ""count"": 0,
            ""next"": null,
            ""previous"": null,
            ""results"": []
        }";

    public static string GetErrorResponse() =>
        @"{ ""error"": ""Invalid request"" }";
}
