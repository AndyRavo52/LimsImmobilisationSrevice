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
    public class ObjetIndisponibiliteService : IObjetIndisponibiliteService
    {
        private readonly ImmobilisationContext _context;

        // Constructeur : Injection de dépendance du DbContext
        public ObjetIndisponibiliteService(ImmobilisationContext context)
        {
            _context = context;
        }

        // Compte le nombre total d'objets d'indisponibilité
        public async Task<int> CountObjetsIndisponibiliteAsync()
        {
            return await _context.ObjetIndisponibilites.CountAsync();
        }

        // Récupère une liste paginée d'objets d'indisponibilité
        public async Task<IEnumerable<ObjetIndisponibiliteDto>> GetObjetsIndisponibiliteAsync(int pageIndex, int pageSize)
        {
            var objetsIndisponibilite = await _context.ObjetIndisponibilites
                .OrderBy(o => o.Designation) // Trie par désignation
                .Skip((pageIndex - 1) * pageSize) // Saute les éléments des pages précédentes
                .Take(pageSize) // Prend un nombre limité d'éléments
                .ToListAsync();

            // Convertit les entités en DTOs
            return objetsIndisponibilite.Select(ObjetIndisponibiliteMapper.ToDto);
        }

        // Récupère un objet d'indisponibilité par son ID
        public async Task<ObjetIndisponibiliteDto> GetObjetIndisponibiliteByIdAsync(int id)
        {
            var objetIndisponibilite = await _context.ObjetIndisponibilites.FindAsync(id);
            if (objetIndisponibilite == null)
            {
                throw new Exception("Objet d'indisponibilité non trouvé");
            }

            return ObjetIndisponibiliteMapper.ToDto(objetIndisponibilite);
        }

        // Crée un nouvel objet d'indisponibilité
        public async Task<ObjetIndisponibiliteDto> CreateObjetIndisponibiliteAsync(ObjetIndisponibiliteDto objetIndisponibiliteDto)
        {
            // Convertit le DTO en entité
            var objetIndisponibilite = ObjetIndisponibiliteMapper.ToEntity(objetIndisponibiliteDto);

            // Ajoute l'objet d'indisponibilité à la base de données
            _context.ObjetIndisponibilites.Add(objetIndisponibilite);
            await _context.SaveChangesAsync();

            // Convertit l'entité en DTO pour la réponse
            return ObjetIndisponibiliteMapper.ToDto(objetIndisponibilite);
        }

        // Met à jour un objet d'indisponibilité existant
        public async Task<ObjetIndisponibiliteDto> UpdateObjetIndisponibiliteAsync(int id, ObjetIndisponibiliteDto objetIndisponibiliteDto)
        {
            // Récupère l'objet d'indisponibilité existant
            var objetIndisponibilite = await _context.ObjetIndisponibilites.FindAsync(id);
            if (objetIndisponibilite == null)
            {
                throw new Exception("Objet d'indisponibilité non trouvé");
            }

            // Met à jour les propriétés de l'objet d'indisponibilité
            objetIndisponibilite.Designation = objetIndisponibiliteDto.Designation;

            // Sauvegarde les modifications
            await _context.SaveChangesAsync();

            // Convertit l'entité en DTO pour la réponse
            return ObjetIndisponibiliteMapper.ToDto(objetIndisponibilite);
        }

        // Supprime un objet d'indisponibilité
        public async Task<bool> DeleteObjetIndisponibiliteAsync(int id)
        {
            // Récupère l'objet d'indisponibilité à supprimer
            var objetIndisponibilite = await _context.ObjetIndisponibilites.FindAsync(id);
            if (objetIndisponibilite == null)
            {
                return false; // Objet d'indisponibilité non trouvé
            }

            // Supprime l'objet d'indisponibilité
            _context.ObjetIndisponibilites.Remove(objetIndisponibilite);
            await _context.SaveChangesAsync();

            return true; // Suppression réussie
        }
    }
}