using LimsImmobilisationService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Services
{
    public interface IEntreeImmobilisationService
    {
        // Compte le nombre total d'entrées d'immobilisations
        Task<int> CountEntreeImmobilisationsAsync();

        // Récupère une liste paginée d'entrées d'immobilisations
        Task<IEnumerable<EntreeImmobilisationDto>> GetEntreeImmobilisationsAsync(int pageIndex, int pageSize);

        // Récupère une entrée d'immobilisation par son ID
        Task<EntreeImmobilisationDto> GetEntreeImmobilisationByIdAsync(int id);

        // Crée une nouvelle entrée d'immobilisation
        Task<EntreeImmobilisationDto> CreateEntreeImmobilisationAsync(EntreeImmobilisationDto entreeImmobilisationDto);

        // Récupère les entrées d'immobilisations non immatriculées
        Task<IEnumerable<EntreeImmobilisationDto>> GetEntreeImmobilisationsNonImmatriculeesAsync();
    }
}