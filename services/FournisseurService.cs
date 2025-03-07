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
    public class FournisseurService : IFournisseurService
    {
        private readonly ImmobilisationContext _context;

        // Constructeur : Injection de dépendance du DbContext
        public FournisseurService(ImmobilisationContext context)
        {
            _context = context;
        }

        // Compte le nombre total de fournisseurs
        public async Task<int> CountFournisseursAsync()
        {
            return await _context.Fournisseurs.CountAsync();
        }

        // Récupère une liste paginée de fournisseurs
        public async Task<IEnumerable<FournisseurDto>> GetFournisseursAsync(int pageIndex, int pageSize)
        {
            var fournisseurs = await _context.Fournisseurs
                .OrderBy(f => f.Designation) // Trie par désignation
                .Skip((pageIndex - 1) * pageSize) // Saute les éléments des pages précédentes
                .Take(pageSize) // Prend un nombre limité d'éléments
                .ToListAsync();

            // Convertit les entités en DTOs
            return fournisseurs.Select(FournisseurMapper.ToDto);
        }

        // Récupère un fournisseur par son ID
        public async Task<FournisseurDto> GetFournisseurByIdAsync(int id)
        {
            var fournisseur = await _context.Fournisseurs.FindAsync(id);
            if (fournisseur == null)
            {
                throw new Exception("Fournisseur non trouvé");
            }

            return FournisseurMapper.ToDto(fournisseur);
        }

        // Crée un nouveau fournisseur
        public async Task<FournisseurDto> CreateFournisseurAsync(FournisseurDto fournisseurDto)
        {
            // Convertit le DTO en entité
            var fournisseur = FournisseurMapper.ToEntity(fournisseurDto);

            // Ajoute le fournisseur à la base de données
            _context.Fournisseurs.Add(fournisseur);
            await _context.SaveChangesAsync();

            // Convertit l'entité en DTO pour la réponse
            return FournisseurMapper.ToDto(fournisseur);
        }

        // Met à jour un fournisseur existant
        public async Task<FournisseurDto> UpdateFournisseurAsync(int id, FournisseurDto fournisseurDto)
        {
            // Récupère le fournisseur existant
            var fournisseur = await _context.Fournisseurs.FindAsync(id);
            if (fournisseur == null)
            {
                throw new Exception("Fournisseur non trouvé");
            }

            // Met à jour les propriétés du fournisseur
            fournisseur.Designation = fournisseurDto.Designation;

            // Sauvegarde les modifications
            await _context.SaveChangesAsync();

            // Convertit l'entité en DTO pour la réponse
            return FournisseurMapper.ToDto(fournisseur);
        }

        // Supprime un fournisseur
        public async Task<bool> DeleteFournisseurAsync(int id)
        {
            // Récupère le fournisseur à supprimer
            var fournisseur = await _context.Fournisseurs.FindAsync(id);
            if (fournisseur == null)
            {
                return false; // Fournisseur non trouvé
            }

            // Supprime le fournisseur
            _context.Fournisseurs.Remove(fournisseur);
            await _context.SaveChangesAsync();

            return true; // Suppression réussie
        }
    }
}