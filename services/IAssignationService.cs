using LimsImmobilisationService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Services
{
    public interface IAssignationService
    {
        Task<int> CountAssignationsAsync();
        Task<int> CountAssignationsAsync(string searchTerm);
        Task<IEnumerable<AssignationDto>> GetAssignationsAsync(int pageIndex, int pageSize);
        Task<AssignationDto?> GetAssignationByIdAsync(int id);
        Task<AssignationDto> CreateAssignationAsync(AssignationDto assignationDto);
        Task<EmployeDto> GetEmployeByMatriculeAsync(string matricule);
        Task<AssignationDto?> UpdateAssignationAsync(int id, AssignationDto assignationDto);
        Task<IEnumerable<ImmobilisationImmatriculationDto>> GetAvailableImmobilisationsAsync();
        Task<IEnumerable<AssignationDto>> SearchAssignationsAsync(string searchTerm);
        Task<IEnumerable<AssignationDto>> SearchAssignationsAsync(string searchTerm, int pageIndex, int pageSize);
        Task<IEnumerable<LocalisationDto>> GetLocalisationsAsync();
        Task<IEnumerable<ImmobilisationImmatriculationDto>> GetAssignedImmobilisationsAsync();
        Task<AssignationDto?> GetCurrentAssignationByImmobilisationIdAsync(int idImmobilisationPropre);
    }
}