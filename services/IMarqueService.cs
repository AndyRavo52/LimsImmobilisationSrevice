using LimsImmobilisationService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Services
{
    public interface IMarqueService
    {
        // Compte le nombre total de marques
        Task<int> CountMarquesAsync();

        // Récupère une liste paginée de marques
        Task<IEnumerable<MarqueDto>> GetMarquesAsync(int pageIndex, int pageSize);

        // Récupère une marque par son ID
        Task<MarqueDto> GetMarqueByIdAsync(int id);

        // Crée une nouvelle marque
        Task<MarqueDto> CreateMarqueAsync(MarqueDto marqueDto);

        // Met à jour une marque existante
        Task<MarqueDto> UpdateMarqueAsync(int id, MarqueDto marqueDto);

        // Supprime une marque
        Task<bool> DeleteMarqueAsync(int id);
    }
}