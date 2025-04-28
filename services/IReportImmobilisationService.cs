using LimsImmobilisationService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Services
{
    public interface IReportImmobilisationService
    {
        // Compte le nombre total de rapports d'immobilisation
        Task<int> CountReportImmobilisationsAsync();

        // Récupère une liste paginée de rapports d'immobilisation
        Task<IEnumerable<ReportImmobilisationDto>> GetReportImmobilisationsAsync(int pageIndex, int pageSize);

        // Récupère un rapport d'immobilisation par son ID
        Task<ReportImmobilisationDto> GetReportImmobilisationByIdAsync(int id);

        // Crée un nouveau rapport d'immobilisation
        Task<ReportImmobilisationDto> CreateReportImmobilisationAsync(ReportImmobilisationDto reportDto);


    }
}
