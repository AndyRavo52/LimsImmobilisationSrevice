using System.Text.Json.Serialization;

namespace LimsImmobilisationService.Dtos
{
    public class ReportImmobilisationDto
    {
        [JsonPropertyName("idReportImmobilisation")]
        public int IdReportImmobilisation { get; set; }

        [JsonPropertyName("dateReport")]
        public DateTime DateReport { get; set; }

        [JsonPropertyName("quantite")]
        public double Quantite { get; set; }

        [JsonPropertyName("idImmobilisation")]
        public int? IdImmobilisation { get; set; }

        [JsonPropertyName("immobilisation")]
        public ImmobilisationDto? Immobilisation { get; set; }
    }
}
