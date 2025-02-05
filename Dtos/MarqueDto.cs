using System.Text.Json.Serialization;

namespace LimsImmobilisationService.Dtos
{
    public class MarqueDto
    {
        [JsonPropertyName("idMarque")]
        public int IdMarque { get; set; }

        [JsonPropertyName("designation")]
        public required string Designation { get; set; }
    }
}