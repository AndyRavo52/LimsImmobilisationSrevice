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
    public class MarqueService : IMarqueService
    {
        private readonly ImmobilisationContext _context;

        // Constructeur : Injection de dépendance du DbContext
        public MarqueService(ImmobilisationContext context)
        {
            _context = context;
        }

        // Compte le nombre total de marques
        public async Task<int> CountMarquesAsync()
        {
            return await _context.Marques.CountAsync();
        }

        // Récupère une liste paginée de marques
        public async Task<IEnumerable<MarqueDto>> GetMarquesAsync(int pageIndex, int pageSize)
        {
            var marques = await _context.Marques
                .OrderBy(m => m.Designation) // Trie par désignation
                .Skip((pageIndex - 1) * pageSize) // Saute les éléments des pages précédentes
                .Take(pageSize) // Prend un nombre limité d'éléments
                .ToListAsync();

            // Convertit les entités en DTOs
            return marques.Select(MarqueMapper.ToDto);
        }

        // Récupère une marque par son ID
        public async Task<MarqueDto> GetMarqueByIdAsync(int id)
        {
            var marque = await _context.Marques.FindAsync(id);
            if (marque == null)
            {
                throw new Exception("Marque non trouvée");
            }

            return MarqueMapper.ToDto(marque);
        }

        // Crée une nouvelle marque
        public async Task<MarqueDto> CreateMarqueAsync(MarqueDto marqueDto)
        {
            // Convertit le DTO en entité
            var marque = MarqueMapper.ToEntity(marqueDto);

            // Ajoute la marque à la base de données
            _context.Marques.Add(marque);
            await _context.SaveChangesAsync();

            // Convertit l'entité en DTO pour la réponse
            return MarqueMapper.ToDto(marque);
        }

        // Met à jour une marque existante
        public async Task<MarqueDto> UpdateMarqueAsync(int id, MarqueDto marqueDto)
        {
            // Récupère la marque existante
            var marque = await _context.Marques.FindAsync(id);
            if (marque == null)
            {
                throw new Exception("Marque non trouvée");
            }

            // Met à jour les propriétés de la marque
            marque.Designation = marqueDto.Designation;

            // Sauvegarde les modifications
            await _context.SaveChangesAsync();

            // Convertit l'entité en DTO pour la réponse
            return MarqueMapper.ToDto(marque);
        }

        // Supprime une marque
        public async Task<bool> DeleteMarqueAsync(int id)
        {
            // Récupère la marque à supprimer
            var marque = await _context.Marques.FindAsync(id);
            if (marque == null)
            {
                return false; // Marque non trouvée
            }

            // Supprime la marque
            _context.Marques.Remove(marque);
            await _context.SaveChangesAsync();

            return true; // Suppression réussie
        }
    }
}