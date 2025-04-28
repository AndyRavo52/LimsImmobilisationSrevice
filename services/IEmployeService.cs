using LimsImmobilisationService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Services
{
    public interface IEmployeService
    {
        // Compte le nombre total d'employés
        Task<int> CountEmployesAsync();

        // Récupère une liste paginée d'employés
        Task<IEnumerable<EmployeDto>> GetEmployesAsync(int pageIndex, int pageSize);

        // Récupère un employé par son ID
        Task<EmployeDto> GetEmployeByIdAsync(int id);

        // Recherche des employés en fonction d'un terme (sur le nom ou le matricule, par exemple)
        Task<IEnumerable<EmployeDto>> SearchEmployesAsync(string searchTerm);
    }
}
