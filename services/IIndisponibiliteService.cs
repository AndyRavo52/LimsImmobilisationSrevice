using LimsImmobilisationService.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Services
{
    public interface IIndisponibiliteService
    {
        // Compte le nombre total d'indisponibilités
        Task<int> CountIndisponibilitesAsync();

        // Récupère une liste paginée d'indisponibilités
        Task<IEnumerable<IndisponibiliteDto>> GetIndisponibilitesAsync(int pageIndex, int pageSize);

        // Récupère une indisponibilité par son ID
        Task<IndisponibiliteDto> GetIndisponibiliteByIdAsync(int id);

        // Crée une nouvelle indisponibilité
        Task<IndisponibiliteDto> CreateIndisponibiliteAsync(IndisponibiliteDto indisponibiliteDto);
        // filtre sur immobilisation immatriculé 
Task<IEnumerable<ImmobilisationImmatriculationDto>> GetAvailableImmobilisationsImmatriculeesAsync();

        // Recherche des indisponibilités en fonction d'un terme (par exemple sur les dates ou les objets)
        // Task<IEnumerable<IndisponibiliteDto>> SearchIndisponibilitesAsync(string searchTerm);
    }
}