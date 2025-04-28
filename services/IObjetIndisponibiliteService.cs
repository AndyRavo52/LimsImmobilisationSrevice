using LimsImmobilisationService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Services
{
    public interface IObjetIndisponibiliteService
    {
        // Compte le nombre total d'objets d'indisponibilité
        Task<int> CountObjetsIndisponibiliteAsync();

        // Récupère une liste paginée d'objets d'indisponibilité
        Task<IEnumerable<ObjetIndisponibiliteDto>> GetObjetsIndisponibiliteAsync(int pageIndex, int pageSize);

        // Récupère un objet d'indisponibilité par son ID
        Task<ObjetIndisponibiliteDto> GetObjetIndisponibiliteByIdAsync(int id);

        // Crée un nouvel objet d'indisponibilité
        Task<ObjetIndisponibiliteDto> CreateObjetIndisponibiliteAsync(ObjetIndisponibiliteDto objetIndisponibiliteDto);

        // Met à jour un objet d'indisponibilité existant
        Task<ObjetIndisponibiliteDto> UpdateObjetIndisponibiliteAsync(int id, ObjetIndisponibiliteDto objetIndisponibiliteDto);

        // Supprime un objet d'indisponibilité
        Task<bool> DeleteObjetIndisponibiliteAsync(int id);
    }
}