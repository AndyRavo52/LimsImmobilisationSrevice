using System;
using System.Text.Json.Serialization;
namespace LimsImmobilisationService.Dtos
{
    public class EntreeImmobilisationAvecResteDto : EntreeImmobilisationDto
    {
        public int ResteImmatriculations { get; set; }
    }
}