# PokemonAPI

Uma API web para gerenciar e recuperar dados de Pokémon.

## Tecnologias Utilizadas
- **Linguagem**: C#
- **Framework**: .NET 8
- **Tecnologias**: ASP.NET Core, Entity Framework Core, HttpClient, IMemoryCache

## Como foi o desenvolvimento desta atividade

Foi investigada a API https://pokeapi.co/api/v2/ para descobrir quais entidades seriam usadas no projeto e as mesmas foram criadas utilizando de ferramentas de conversão de json para DTO C#.
Posteriormente foi dado seguimento a implementação de acordo com as tarefas demandadas no teste codificando Services e Controllers para tal.

## Instalação e Uso

### Pré-requisitos
- SDK do .NET 8
- SQLite

### Passos

1. **Clone o repositório**   
git clone https://github.com/thiagom3005/PokemonAPI.git cd PokemonAPI

2. **Configure o banco de dados**
   - Atualize a string de conexão no `appsettings.json` para apontar para o seu banco de dados.
        - Atualize também no `PokemonContext.cs` e `PokemonContextFactory.cs`
   - Execute o seguinte comando para aplicar as migrações:
        dotnet ef database update

3. **Compile o projeto**
   dotnet build

4. **Execute o projeto**
   dotnet run --project PokemonAPI.WebAPI

5. **Acesse a API**
   - A API estará disponível em `https://localhost:5001` ou `http://localhost:5000`.

### Usando a API
- Utilize ferramentas como Postman ou cURL para interagir com os endpoints da API.
- Exemplos de endpoints:
  - `GET /api/pokemon` - Recupera uma lista de Pokémon.
  - `POST /api/pokemonmaster` - Adiciona um novo mestre Pokémon.

### Endpoints

#### PokemonController

- **`GET /api/pokemon`**
  - Descrição: Recupera uma lista de Pokémon.
  - Parâmetros:
    - `limit` (opcional): Número de Pokémon a serem retornados (padrão: 20).
    - `offset` (opcional): Ponto de partida para a listagem (padrão: 0).
    - `fetchDetails` (opcional): Se deve buscar detalhes adicionais (padrão: true).
  - Resposta: 
     {
        "data": [
            {
            "id": 1,
            "name": "bulbasaur",
            "sprites": { ... },
            "url": "https://pokeapi.co/api/v2/pokemon/1/",
            "evolutions": [ ... ],
            "spriteBase64": "..."
            }
        ],
        "totalCount": 1118,
        "pageSize": 20,
        "currentPage": 1,
        "nextPage": "https://pokeapi.co/api/v2/pokemon?offset=20&limit=20",
        "previousPage": null
     }

- **`GET /api/pokemon/{id}`**
  - Descrição: Recupera um Pokémon pelo seu ID.
  - Parâmetros:
    - `id`: ID do Pokémon.
  - Resposta: 
    {
        "id": 1,
        "name": "bulbasaur",
        "sprites": { ... },
        "url": "https://pokeapi.co/api/v2/pokemon/1/",
        "evolutions": [ ... ],
        "spriteBase64": "..."
    }

- **`GET /api/pokemon/random`**
  - Descrição: Recupera uma lista de Pokémon aleatórios.
  - Parâmetros:
    - `count` (opcional): Número de Pokémon a serem retornados (padrão: 10).
  - Resposta: 
    [
        {
            "id": 1,
            "name": "bulbasaur",
            "sprites": { ... },
            "url": "https://pokeapi.co/api/v2/pokemon/1/",
            "evolutions": [ ... ],
            "spriteBase64": "..."
        }
    ]

- **`POST /api/pokemon/capture`**
  - Descrição: Captura um Pokémon.
  - Parâmetros:
    - Corpo da requisição: 
    {
        "pokemonId": 1
    }
  - Resposta: 
    {
        "message": "Pokémon capturado com sucesso."
    }

- **`GET /api/pokemon/captured`**
  - Descrição: Lista todos os Pokémon capturados.
  - Resposta: 
    [
        {
            "id": 1,
            "pokemonId": 1
        }
    ]

#### PokemonMasterController

- **`POST /api/pokemonmaster`**
  - Descrição: Adiciona um novo mestre Pokémon.
  - Parâmetros:
    - Corpo da requisição: 
    {
        "name": "Ash Ketchum",
        "age": 10,
        "cpf": "123.456.789-00"
    }
  - Resposta: 
    {
        "message": "Mestre Pokémon registrado com sucesso.",
        "pokemonMaster": {
            "id": 1,
            "name": "Ash Ketchum",
            "age": 10,
            "cpf": "123.456.789-00"
        }
    }

Para mais detalhes sobre a documentação da API, consulte o Swagger UI disponível em `https://localhost:5001/swagger`.


   
     
     
