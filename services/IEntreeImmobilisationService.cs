using LimsImmobilisationService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Services
{
    public interface IEntreeImmobilisationService
    {
        Task<int> CountEntreeImmobilisationsAsync();
        Task<IEnumerable<EntreeImmobilisationDto>> GetEntreeImmobilisationsAsync(int pageIndex, int pageSize);
        Task<EntreeImmobilisationDto> GetEntreeImmobilisationByIdAsync(int id);
        Task<EntreeImmobilisationDto> CreateEntreeImmobilisationAsync(EntreeImmobilisationDto entreeImmobilisationDto);
        Task<IEnumerable<EntreeImmobilisationDto>> GetEntreeImmobilisationsNonImmatriculeesAsync();
        Task<Dictionary<string, decimal>> GetDepensesParMoisAsync(int annee);
    }
}