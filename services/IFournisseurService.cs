using LimsImmobilisationService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Services
{
    public interface IFournisseurService
    {
        // Compte le nombre total de fournisseurs
        Task<int> CountFournisseursAsync();

        // Récupère une liste paginée de fournisseurs
        Task<IEnumerable<FournisseurDto>> GetFournisseursAsync(int pageIndex, int pageSize);

        // Récupère un fournisseur par son ID
        Task<FournisseurDto> GetFournisseurByIdAsync(int id);

        // Crée un nouveau fournisseur
        Task<FournisseurDto> CreateFournisseurAsync(FournisseurDto fournisseurDto);

        // Met à jour un fournisseur existant
        Task<FournisseurDto> UpdateFournisseurAsync(int id, FournisseurDto fournisseurDto);

        // Supprime un fournisseur
        Task<bool> DeleteFournisseurAsync(int id);
    }
}