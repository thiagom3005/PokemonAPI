using System.Text.Json.Serialization;
namespace PokemonAPI.Domain.Models.EvolutionChain
{

    public class EvolutionDetail
    {
        [JsonPropertyName("gender")]
        public int? Gender { get; set; }

        [JsonPropertyName("held_item")]
        public object HeldItem { get; set; }

        [JsonPropertyName("item")]
        public object Item { get; set; }

        [JsonPropertyName("known_move")]
        public object KnownMove { get; set; }

        [JsonPropertyName("known_move_type")]
        public object KnownMoveType { get; set; }

        [JsonPropertyName("location")]
        public object Location { get; set; }

        [JsonPropertyName("min_affection")]
        public int? MinAffection { get; set; }

        [JsonPropertyName("min_beauty")]
        public int? MinBeauty { get; set; }

        [JsonPropertyName("min_happiness")]
        public int? MinHappiness { get; set; }

        [JsonPropertyName("min_level")]
        public int? MinLevel { get; set; }

        [JsonPropertyName("needs_overworld_rain")]
        public bool NeedsOverworldRain { get; set; }

        [JsonPropertyName("party_species")]
        public object PartySpecies { get; set; }

        [JsonPropertyName("party_type")]
        public object PartyType { get; set; }

        [JsonPropertyName("relative_physical_stats")]
        public int? RelativePhysicalStats { get; set; }

        [JsonPropertyName("time_of_day")]
        public string TimeOfDay { get; set; }

        [JsonPropertyName("trade_species")]
        public object TradeSpecies { get; set; }

        [JsonPropertyName("trigger")]
        public Trigger Trigger { get; set; }

        [JsonPropertyName("turn_upside_down")]
        public bool TurnUpsideDown { get; set; }
    }

}