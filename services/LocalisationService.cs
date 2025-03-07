using LimsImmobilisationService.Data;
using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Mappers;
using LimsImmobilisationService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Services
{
    public class LocalisationService : ILocalisationService
    {
        private readonly ImmobilisationContext _context;

        // Constructeur : Injection de dépendance du DbContext
        public LocalisationService(ImmobilisationContext context)
        {
            _context = context;
        }

        // Compte le nombre total de localisations
        public async Task<int> CountLocalisationsAsync()
        {
            return await _context.Localisations.CountAsync();
        }

        // Récupère une liste paginée de localisations
        public async Task<IEnumerable<LocalisationDto>> GetLocalisationsAsync(int pageIndex, int pageSize)
        {
            var localisations = await _context.Localisations
                .OrderBy(l => l.Designation) // Trie par désignation
                .Skip((pageIndex - 1) * pageSize) // Saute les éléments des pages précédentes
                .Take(pageSize) // Prend un nombre limité d'éléments
                .ToListAsync();

            // Convertit les entités en DTOs
            return localisations.Select(LocalisationMapper.ToDto);
        }

        // Récupère une localisation par son ID
        public async Task<LocalisationDto> GetLocalisationByIdAsync(int id)
        {
            var localisation = await _context.Localisations.FindAsync(id);
            if (localisation == null)
            {
                throw new Exception("Localisation non trouvée");
            }

            return LocalisationMapper.ToDto(localisation);
        }

        // Crée une nouvelle localisation
        public async Task<LocalisationDto> CreateLocalisationAsync(LocalisationDto localisationDto)
        {
            // Convertit le DTO en entité
            var localisation = LocalisationMapper.ToEntity(localisationDto);

            // Ajoute la localisation à la base de données
            _context.Localisations.Add(localisation);
            await _context.SaveChangesAsync();

            // Convertit l'entité en DTO pour la réponse
            return LocalisationMapper.ToDto(localisation);
        }

        // Met à jour une localisation existante
        public async Task<LocalisationDto> UpdateLocalisationAsync(int id, LocalisationDto localisationDto)
        {
            // Récupère la localisation existante
            var localisation = await _context.Localisations.FindAsync(id);
            if (localisation == null)
            {
                throw new Exception("Localisation non trouvée");
            }

            // Met à jour les propriétés de la localisation
            localisation.Designation = localisationDto.Designation;

            // Sauvegarde les modifications
            await _context.SaveChangesAsync();

            // Convertit l'entité en DTO pour la réponse
            return LocalisationMapper.ToDto(localisation);
        }

        // Supprime une localisation
        public async Task<bool> DeleteLocalisationAsync(int id)
        {
            // Récupère la localisation à supprimer
            var localisation = await _context.Localisations.FindAsync(id);
            if (localisation == null)
            {
                return false; // Localisation non trouvée
            }

            // Supprime la localisation
            _context.Localisations.Remove(localisation);
            await _context.SaveChangesAsync();

            return true; // Suppression réussie
        }
    }
}