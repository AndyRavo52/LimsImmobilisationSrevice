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
    public class IndisponibiliteService : IIndisponibiliteService
    {
        private readonly ImmobilisationContext _context;

        // Constructeur : Injection de dépendance du DbContext
        public IndisponibiliteService(ImmobilisationContext context)
        {
            _context = context;
        }

        // Compte le nombre total d'indisponibilités
        public async Task<int> CountIndisponibilitesAsync()
        {
            return await _context.Indisponibilites.CountAsync();
        }

        // Récupère une liste paginée d'indisponibilités
      public async Task<IEnumerable<IndisponibiliteDto>> GetIndisponibilitesAsync(int pageIndex, int pageSize)
{
    var indisponibilites = await _context.Indisponibilites
        .Include(i => i.ImmobilisationImmatriculation)
            .ThenInclude(im => im.EntreeImmobilisation)
                .ThenInclude(ei => ei.Immobilisation)
        .Include(i => i.ObjetIndisponibilite)
        .OrderBy(i => i.DateDebut)
        .Skip((pageIndex - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return indisponibilites.Select(IndisponibiliteMapper.ToDto);
}

        // Récupère une indisponibilité par son ID
        public async Task<IndisponibiliteDto> GetIndisponibiliteByIdAsync(int id)
        {
            var indisponibilite = await _context.Indisponibilites
                .Include(i => i.ImmobilisationImmatriculation) // Charge l'immatriculation associée
                .Include(i => i.ObjetIndisponibilite) // Charge l'objet d'indisponibilité associé
                .FirstOrDefaultAsync(i => i.IdIndisponibilite == id);

            if (indisponibilite == null)
            {
                throw new Exception("Indisponibilité non trouvée");
            }

            // Convertit l'entité en DTO
            return IndisponibiliteMapper.ToDto(indisponibilite);
        }

        // Crée une nouvelle indisponibilité
        public async Task<IndisponibiliteDto> CreateIndisponibiliteAsync(IndisponibiliteDto indisponibiliteDto)
        {
            // Convertit le DTO en entité
            var indisponibilite = IndisponibiliteMapper.ToEntity(indisponibiliteDto);

            // Ajoute l'indisponibilité à la base de données
            _context.Indisponibilites.Add(indisponibilite);
            await _context.SaveChangesAsync();

            // Charge les relations pour inclure dans le DTO de retour
            await _context.Entry(indisponibilite)
                .Reference(i => i.ImmobilisationImmatriculation)
                .LoadAsync();
            await _context.Entry(indisponibilite)
                .Reference(i => i.ObjetIndisponibilite)
                .LoadAsync();

            // Convertit l'entité en DTO pour la réponse
            return IndisponibiliteMapper.ToDto(indisponibilite);
        }

        // Dans IndisponibiliteService.cs
        public async Task<IEnumerable<ImmobilisationImmatriculationDto>> GetAvailableImmobilisationsImmatriculeesAsync()
        {
            var today = DateTime.Today;

            // Récupérer les IDs des immobilisations actuellement indisponibles
            var indisponiblesIds = await _context.Indisponibilites
                .Where(i => i.DateFin >= today)
                .Select(i => i.IdImmobilisationPropre)
                .Distinct()
                .ToListAsync();

            // Récupérer les immobilisations immatriculées qui ne sont pas dans la liste des indisponibles
            var availableImmobilisations = await _context.ImmobilisationImmatriculations
                .Where(im => !indisponiblesIds.Contains(im.IdImmobilisationPropre))
                .Include(im => im.EntreeImmobilisation)
                    .ThenInclude(ei => ei.Immobilisation)
                .ToListAsync();

            // Convertir en DTO
            return availableImmobilisations
                .Where(im => im != null)
                .Select(im => ImmobilisationImmatriculationMapper.ToDto(im)!)
                .Where(dto => dto != null)
                .Cast<ImmobilisationImmatriculationDto>();
        }
    }
}