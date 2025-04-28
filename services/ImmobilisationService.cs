using LimsImmobilisationService.Data;
using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Mappers;
using LimsImmobilisationService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Services
{
    public class ImmobilisationService : IImmobilisationService
    {
        private readonly ImmobilisationContext _context;

        // Constructeur : Injection de dépendance du DbContext
        public ImmobilisationService(ImmobilisationContext context)
        {
            _context = context;
        }

        // Compte le nombre total d'immobilisations
        public async Task<int> CountImmobilisationsAsync()
        {
            return await _context.Immobilisations.CountAsync();
        }

        // Récupère une liste paginée d'immobilisations
        public async Task<IEnumerable<ImmobilisationDto>> GetImmobilisationsAsync(int pageIndex, int pageSize)
        {
            var immobilisations = await _context.Immobilisations
                .OrderByDescending(i => i.IdImmobilisation) // Trie par ID en ordre décroissant
                .Skip((pageIndex - 1) * pageSize) // Saute les éléments des pages précédentes
                .Take(pageSize) // Prend un nombre limité d'éléments
                .ToListAsync();

            // Convertit les entités en DTOs
            return immobilisations.Select(ImmobilisationMapper.ToDto);
        }

        // Récupère une immobilisation par son ID
        public async Task<ImmobilisationDto> GetImmobilisationByIdAsync(int id)
        {
            var immobilisation = await _context.Immobilisations
                .Include(i => i.Marque) // Charge la marque associée
                .FirstOrDefaultAsync(i => i.IdImmobilisation == id);

            if (immobilisation == null)
            {
                throw new Exception("Immobilisation non trouvée");
            }

            // Convertit l'entité en DTO
            return ImmobilisationMapper.ToDto(immobilisation);
        }

        // Crée une nouvelle immobilisation
        public async Task<ImmobilisationDto> CreateImmobilisationAsync(ImmobilisationDto immobilisationDto)
        {
            // Convertit le DTO en entité
            var immobilisation = ImmobilisationMapper.ToEntity(immobilisationDto);

            // Ajoute l'immobilisation à la base de données
            _context.Immobilisations.Add(immobilisation);
            await _context.SaveChangesAsync();

            // Convertit l'entité en DTO pour la réponse
            return ImmobilisationMapper.ToDto(immobilisation);
        }

        // Met à jour une immobilisation existante
        public async Task<ImmobilisationDto> UpdateImmobilisationAsync(int id, ImmobilisationDto immobilisationDto)
        {
            // Récupère l'immobilisation existante
            var immobilisation = await _context.Immobilisations
                .FirstOrDefaultAsync(i => i.IdImmobilisation == id);

            if (immobilisation == null)
            {
                throw new Exception("Immobilisation non trouvée");
            }

            // Met à jour les propriétés de l'immobilisation
            immobilisation.Reference = immobilisationDto.Reference;
            immobilisation.Designation = immobilisationDto.Designation;
            immobilisation.IdMarque = immobilisationDto.IdMarque;

            // Sauvegarde les modifications
            await _context.SaveChangesAsync();

            // Convertit l'entité en DTO pour la réponse
            return ImmobilisationMapper.ToDto(immobilisation);
        }

        // Supprime une immobilisation
        public async Task<bool> DeleteImmobilisationAsync(int id)
        {
            // Récupère l'immobilisation à supprimer
            var immobilisation = await _context.Immobilisations
                .FirstOrDefaultAsync(i => i.IdImmobilisation == id);

            if (immobilisation == null)
            {
                return false; // Immobilisation non trouvée
            }

            // Supprime l'immobilisation
            _context.Immobilisations.Remove(immobilisation);
            await _context.SaveChangesAsync();

            return true; // Suppression réussie
        }

        // Recherche des immobilisations en fonction d'un terme (sur la désignation)
        public async Task<IEnumerable<ImmobilisationDto>> SearchImmobilisationsAsync(string searchTerm)
        {
            var query = _context.Immobilisations
                                .Include(i => i.Marque)
                                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                // Vous pouvez adapter le critère de recherche (ici on cherche dans la désignation)
                query = query.Where(i => EF.Functions.Like(i.Designation, $"%{searchTerm}%"));
            }

            var immobilisations = await query
                .OrderBy(i => i.Reference)
                .ToListAsync();

            return immobilisations.Select(ImmobilisationMapper.ToDto);
        }
    }
}