using LimsImmobilisationService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Services
{
    public interface ILocalisationService
    {
        // Compte le nombre total de localisations
        Task<int> CountLocalisationsAsync();

        // Récupère une liste paginée de localisations
        Task<IEnumerable<LocalisationDto>> GetLocalisationsAsync(int pageIndex, int pageSize);

        // Récupère une localisation par son ID
        Task<LocalisationDto?> GetLocalisationByIdAsync(int id);

        // Crée une nouvelle localisation
        Task<LocalisationDto?> CreateLocalisationAsync(LocalisationDto localisationDto);

        // Met à jour une localisation existante
        Task<LocalisationDto?> UpdateLocalisationAsync(int id, LocalisationDto localisationDto);

        // Supprime une localisation
        Task<bool> DeleteLocalisationAsync(int id);
    }
}