using LimsImmobilisationService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Services
{
    public interface IImmobilisationImmatriculationService
    {
        Task<int> CountImmobilisationImmatriculationsAsync();
        Task<IEnumerable<ImmobilisationImmatriculationDto>> GetImmobilisationImmatriculationsAsync(int pageIndex, int pageSize);
        Task<ImmobilisationImmatriculationDto> GetImmobilisationImmatriculationByIdAsync(int id);
        Task<ImmobilisationImmatriculationDto> CreateImmobilisationImmatriculationAsync(ImmobilisationImmatriculationDto immobilisationImmatriculationDto);
    }
}