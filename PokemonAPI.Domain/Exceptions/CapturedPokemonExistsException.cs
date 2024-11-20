namespace PokemonAPI.Domain.Exceptions
{
    public class CapturedPokemonExistsException : Exception
    {
        public CapturedPokemonExistsException() : base("Pokemon has already been captured.") { }
    }
}
