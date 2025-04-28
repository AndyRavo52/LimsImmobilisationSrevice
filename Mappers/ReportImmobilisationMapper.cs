using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Models;

namespace LimsImmobilisationService.Mappers
{
    public static class ReportImmobilisationMapper
    {
        // Convertit une entité en DTO
        public static ReportImmobilisationDto ToDto(ReportImmobilisation report)
        {
            return new ReportImmobilisationDto
            {
                IdReportImmobilisation = report.IdReportImmobilisation,
                DateReport = report.DateReport,
                Quantite = report.Quantite,
                IdImmobilisation = report.IdImmobilisation,
                Immobilisation = report.Immobilisation != null ? ImmobilisationMapper.ToDto(report.Immobilisation) : null
            };
        }

        // Convertit un DTO en entité
        public static ReportImmobilisation ToEntity(ReportImmobilisationDto dto)
        {
            return new ReportImmobilisation
            {
                IdReportImmobilisation = dto.IdReportImmobilisation,
                DateReport = dto.DateReport,
                Quantite = dto.Quantite,
                IdImmobilisation = dto.IdImmobilisation
            };
        }
    }
}
