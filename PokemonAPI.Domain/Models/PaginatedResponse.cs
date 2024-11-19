namespace PokemonAPI.Domain.Models
{
    public class PaginatedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public string NextPage { get; set; }
        public string PreviousPage { get; set; }
    }
}
