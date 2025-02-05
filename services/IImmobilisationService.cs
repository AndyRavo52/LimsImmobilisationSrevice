using LimsImmobilisationService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Services
{
    public interface IImmobilisationService
    {
        // Compte le nombre total d'immobilisations
        Task<int> CountImmobilisationsAsync();

        // Récupère une liste paginée d'immobilisations
        Task<IEnumerable<ImmobilisationDto>> GetImmobilisationsAsync(int pageIndex, int pageSize);

        // Récupère une immobilisation par son ID
        Task<ImmobilisationDto> GetImmobilisationByIdAsync(int id);

        // Crée une nouvelle immobilisation
        Task<ImmobilisationDto> CreateImmobilisationAsync(ImmobilisationDto immobilisationDto);

        // Met à jour une immobilisation existante
        Task<ImmobilisationDto> UpdateImmobilisationAsync(int id, ImmobilisationDto immobilisationDto);

        // Supprime une immobilisation
        Task<bool> DeleteImmobilisationAsync(int id);
    }
}