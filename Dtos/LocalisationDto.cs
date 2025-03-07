using System.Text.Json.Serialization;

namespace LimsImmobilisationService.Dtos
{
    public class LocalisationDto
    {
        [JsonPropertyName("idLocalisation")]
        public int IdLocalisation { get; set; }

        [JsonPropertyName("designation")]
        public required string Designation { get; set; }
    }
}