using System;
using System.Text.Json.Serialization;

namespace Models
{
    public class DataRequest
    {
        [JsonPropertyName("bgg_id")]
        public int BggId { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("yearpublished")]
        public string YearPublished { get; set; }

        [JsonPropertyName("rank")]
        public int? Rank { get; set; }

        [JsonPropertyName("bayesaverage")]
        public double? BayesAverage { get; set; }

        [JsonPropertyName("average")]
        public double? Average { get; set; }

        [JsonPropertyName("usersrated")]
        public int? UsersRated { get; set; }

        [JsonPropertyName("is_expansion")]
        public bool? IsExpansion { get; set; }

        [JsonPropertyName("abstracts_rank")]
        public int? AbstractsRank { get; set; }

        [JsonPropertyName("cgs_rank")]
        public int? CgsRank { get; set; }

        [JsonPropertyName("childrensgames_rank")]
        public int? ChildrensGamesRank { get; set; }

        [JsonPropertyName("familygames_rank")]
        public int? FamilyGamesRank { get; set; }

        [JsonPropertyName("partygames_rank")]
        public int? PartyGamesRank { get; set; }

        [JsonPropertyName("strategygames_rank")]
        public int? StrategyGamesRank { get; set; }

        [JsonPropertyName("thematic_rank")]
        public int? ThematicRank { get; set; }

        [JsonPropertyName("wargames_rank")]
        public int? WarGamesRank { get; set; }

        /* Server-populated */
        [JsonIgnore]
        public DateTime ReceivedAt { get; set; }
    }

    public class CollectionRequest
    {
        // objectid
        [JsonPropertyName("bgg_id")]
        public int BggId { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("yearpublished")]
        public required string YearPublished { get; set; }

        // is_expansion str representation ["expansion", "standalone"]
        [JsonPropertyName("itemtype")]
        public required string ItemType { get; set; }

        [JsonPropertyName("averageweight")]
        public double AverageWeight { get; set; }

        [JsonPropertyName("numowned")]
        public int? NumOwned { get; set; }

        [JsonPropertyName("minplayers")]
        public int MinPlayers { get; set; }

        [JsonPropertyName("maxplayers")]
        public int MaxPlayers { get; set; }

        [JsonPropertyName("minplaytime")]
        public int MinPlayTime { get; set; }

        [JsonPropertyName("maxplaytime")]
        public int MaxPlayTime { get; set; }

        [JsonPropertyName("playingtime")]
        public int PlayingTime { get; set; }

        [JsonPropertyName("bggrecplayers")]
        public required string BggRecPlayers { get; set; }

        [JsonPropertyName("bggbestplayers")]
        public required string BggBestPlayers { get; set; }

        [JsonPropertyName("bggrecagerange")]
        public required string BggRecAgeRange { get; set; }
    }
}