using System.Text.Json.Serialization;

namespace LimsImmobilisationService.Dtos
{
    public class ImmobilisationDto
    {
        [JsonPropertyName("idImmobilisation")]
        public int IdImmobilisation { get; set; }

        [JsonPropertyName("reference")]
        public required string Reference { get; set; }

        [JsonPropertyName("designation")]
        public required string Designation { get; set; }

        [JsonPropertyName("idMarque")]
        public int IdMarque { get; set; }

        [JsonPropertyName("marque")]
        public MarqueDto? Marque { get; set; }
    }
}