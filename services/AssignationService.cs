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
    public class AssignationService : IAssignationService
    {
        private readonly ImmobilisationContext _context;

        public AssignationService(ImmobilisationContext context)
        {
            _context = context;
        }

        public async Task<int> CountAssignationsAsync()
        {
            return await _context.Assignations.CountAsync();
        }

        public async Task<int> CountAssignationsAsync(string searchTerm)
        {
            var query = _context.Assignations
                .Include(a => a.Employe)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(a => a.Employe != null && (a.Employe.Nom + " " + a.Employe.Prenom).Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            return await query.CountAsync();
        }

        public async Task<IEnumerable<AssignationDto>> GetAssignationsAsync(int pageIndex, int pageSize)
        {
            var assignations = await _context.Assignations
                .Include(a => a.Employe)
                .Include(a => a.ImmobilisationImmatriculation)
                .ThenInclude(ii => ii.EntreeImmobilisation)
                .ThenInclude(ei => ei.Immobilisation)
                .Include(a => a.Localisation)
                .OrderBy(a => a.IdAssignation)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return assignations.Select(AssignationMapper.ToDto).Where(dto => dto != null).Cast<AssignationDto>();
        }

        public async Task<AssignationDto?> GetAssignationByIdAsync(int id)
        {
            var assignation = await _context.Assignations
                .Include(a => a.Employe)
                .Include(a => a.ImmobilisationImmatriculation)
                .ThenInclude(ii => ii.EntreeImmobilisation)
                .ThenInclude(ei => ei.Immobilisation)
                .Include(a => a.Localisation)
                .FirstOrDefaultAsync(a => a.IdAssignation == id);

            if (assignation == null)
            {
                return null;
            }

            return AssignationMapper.ToDto(assignation);
        }

        public async Task<AssignationDto> CreateAssignationAsync(AssignationDto assignationDto)
        {
            // Validation des IDs
            if (!await _context.ImmobilisationImmatriculations.AnyAsync(ii => ii.IdImmobilisationPropre == assignationDto.IdImmobilisationPropre))
            {
                throw new ArgumentException("L'immobilisation spécifiée n'existe pas.");
            }
            if (!await _context.Employes.AnyAsync(e => e.IdEmploye == assignationDto.IdEmploye))
            {
                throw new ArgumentException("L'employé spécifié n'existe pas.");
            }
            if (assignationDto.IdLocalisation.HasValue && !await _context.Localisations.AnyAsync(l => l.IdLocalisation == assignationDto.IdLocalisation.Value))
            {
                throw new ArgumentException("La localisation spécifiée n'existe pas.");
            }

            // Utilisation de ToEntitySimple pour éviter les entités imbriquées
            var assignation = AssignationMapper.ToEntitySimple(assignationDto);
            if (assignation == null)
                throw new Exception("Erreur de conversion du DTO en entité");

            _context.Assignations.Add(assignation);
            await _context.SaveChangesAsync();

            // Charger l'assignation complète pour le retour
            var createdAssignation = await _context.Assignations
                .Include(a => a.Employe)
                .Include(a => a.ImmobilisationImmatriculation)
                .ThenInclude(ii => ii.EntreeImmobilisation)
                .ThenInclude(ei => ei.Immobilisation)
                .Include(a => a.Localisation)
                .FirstOrDefaultAsync(a => a.IdAssignation == assignation.IdAssignation);

            if (createdAssignation == null)
                throw new Exception("Erreur lors de la récupération de l'assignation créée");

            var dto = AssignationMapper.ToDto(createdAssignation);
            if (dto == null)
                throw new Exception("Erreur de conversion de l'entité en DTO");

            return dto;
        }

        public async Task<EmployeDto> GetEmployeByMatriculeAsync(string matricule)
        {
            var employe = await _context.Employes
                .FirstOrDefaultAsync(e => e.Matricule == matricule);

            if (employe == null)
            {
                throw new Exception("Employé non trouvé");
            }

            return EmployeMapper.ToDto(employe);
        }

        public async Task<AssignationDto?> UpdateAssignationAsync(int id, AssignationDto assignationDto)
        {
            var assignation = await _context.Assignations.FindAsync(id);
            if (assignation == null)
            {
                return null;
            }

            // Validation des IDs pour la mise à jour
            if (!await _context.Employes.AnyAsync(e => e.IdEmploye == assignationDto.IdEmploye))
            {
                throw new ArgumentException("L'employé spécifié n'existe pas.");
            }
            if (assignationDto.IdLocalisation.HasValue && !await _context.Localisations.AnyAsync(l => l.IdLocalisation == assignationDto.IdLocalisation.Value))
            {
                throw new ArgumentException("La localisation spécifiée n'existe pas.");
            }

            assignation.IdEmploye = assignationDto.IdEmploye;
            assignation.IdLocalisation = assignationDto.IdLocalisation ?? assignation.IdLocalisation;
            assignation.DateAssignation = assignationDto.DateAssignation;

            await _context.SaveChangesAsync();

            var updatedAssignation = await _context.Assignations
                .Include(a => a.Employe)
                .Include(a => a.ImmobilisationImmatriculation)
                .ThenInclude(ii => ii.EntreeImmobilisation)
                .ThenInclude(ei => ei.Immobilisation)
                .Include(a => a.Localisation)
                .FirstOrDefaultAsync(a => a.IdAssignation == id);

            if (updatedAssignation == null)
                return null;

            var dto = AssignationMapper.ToDto(updatedAssignation);
            if (dto == null)
                return null;

            return dto;
        }

        public async Task<IEnumerable<ImmobilisationImmatriculationDto>> GetAvailableImmobilisationsAsync()
        {
            var assignedIds = await _context.Assignations
                .Select(a => a.IdImmobilisationPropre)
                .Distinct()
                .ToListAsync();

            var availableImmobilisations = await _context.ImmobilisationImmatriculations
                .Include(ii => ii.EntreeImmobilisation)
                .ThenInclude(ei => ei.Immobilisation)
                .Where(ii => !assignedIds.Contains(ii.IdImmobilisationPropre))
                .ToListAsync();

            return availableImmobilisations.Select(ImmobilisationImmatriculationMapper.ToDto).Where(dto => dto != null).Cast<ImmobilisationImmatriculationDto>();
        }

        public async Task<IEnumerable<AssignationDto>> SearchAssignationsAsync(string searchTerm)
        {
            var assignations = await _context.Assignations
                .Include(a => a.Employe)
                .Include(a => a.ImmobilisationImmatriculation)
                .ThenInclude(ii => ii.EntreeImmobilisation)
                .ThenInclude(ei => ei.Immobilisation)
                .Include(a => a.Localisation)
                .Where(a => a.Employe != null && (a.Employe.Nom + " " + a.Employe.Prenom).Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            return assignations.Select(AssignationMapper.ToDto).Where(dto => dto != null).Cast<AssignationDto>();
        }

        public async Task<IEnumerable<AssignationDto>> SearchAssignationsAsync(string searchTerm, int pageIndex, int pageSize)
        {
            var query = _context.Assignations
                .Include(a => a.Employe)
                .Include(a => a.ImmobilisationImmatriculation)
                .ThenInclude(ii => ii.EntreeImmobilisation)
                .ThenInclude(ei => ei.Immobilisation)
                .Include(a => a.Localisation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(a => a.Employe != null && (a.Employe.Nom + " " + a.Employe.Prenom).Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            var assignations = await query
                .OrderBy(a => a.IdAssignation)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return assignations.Select(AssignationMapper.ToDto).Where(dto => dto != null).Cast<AssignationDto>();
        }

        public async Task<IEnumerable<LocalisationDto>> GetLocalisationsAsync()
        {
            var localisations = await _context.Localisations
                .OrderBy(l => l.Designation)
                .ToListAsync();

            return localisations.Select(LocalisationMapper.ToDto).Where(dto => dto != null).Cast<LocalisationDto>();
        }

        public async Task<IEnumerable<ImmobilisationImmatriculationDto>> GetAssignedImmobilisationsAsync()
        {
            var assignedImmobilisations = await _context.Assignations
                .Include(a => a.ImmobilisationImmatriculation)
                .ThenInclude(ii => ii.EntreeImmobilisation)
                .ThenInclude(ei => ei.Immobilisation)
                .Select(a => a.ImmobilisationImmatriculation)
                .Distinct()
                .OrderBy(ii => ii != null && ii.EntreeImmobilisation != null && ii.EntreeImmobilisation.Immobilisation != null ? ii.EntreeImmobilisation.Immobilisation.Designation : string.Empty)
                .ToListAsync();

            return assignedImmobilisations
                .Where(ii => ii != null)
                .Select(ii => ImmobilisationImmatriculationMapper.ToDto(ii)!)
                .Where(dto => dto != null)
                .Cast<ImmobilisationImmatriculationDto>();
        }

        public async Task<AssignationDto?> GetCurrentAssignationByImmobilisationIdAsync(int idImmobilisationPropre)
        {
            var assignation = await _context.Assignations
                .Include(a => a.Employe)
                .Include(a => a.ImmobilisationImmatriculation)
                .ThenInclude(ii => ii.EntreeImmobilisation)
                .ThenInclude(ei => ei.Immobilisation)
                .Include(a => a.Localisation)
                .OrderByDescending(a => a.DateAssignation)
                .FirstOrDefaultAsync(a => a.IdImmobilisationPropre == idImmobilisationPropre);

            return assignation != null ? AssignationMapper.ToDto(assignation) : null;
        }
    }
}