using LimsImmobilisationService.Data;
using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Services
{
    public class ImmobilisationImmatriculationService : IImmobilisationImmatriculationService
    {
        private readonly ImmobilisationContext _context;

        public ImmobilisationImmatriculationService(ImmobilisationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> CountImmobilisationImmatriculationsAsync()
        {
            return await _context.ImmobilisationImmatriculations.CountAsync();
        }

        public async Task<IEnumerable<ImmobilisationImmatriculationDto>> GetImmobilisationImmatriculationsAsync(int pageIndex, int pageSize)
{
    if (pageIndex < 1) throw new ArgumentException("L'index de page doit être supérieur ou égal à 1.", nameof(pageIndex));
    if (pageSize < 1) throw new ArgumentException("La taille de page doit être supérieure ou égale à 1.", nameof(pageSize));

    var immobilisationImmatriculations = await _context.ImmobilisationImmatriculations
        .Include(ip => ip.EntreeImmobilisation) // Inclure l'EntreeImmobilisation
            .ThenInclude(ei => ei.Immobilisation) // PUIS inclure l'Immobilisation liée à l'EntreeImmobilisation
        .OrderBy(ip => ip.IdImmobilisationPropre) // Ou un autre ordre pertinent
        .Skip((pageIndex - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    // Assurez-vous que vos Mappers gèrent correctement les éventuels nulls,
    // mais avec le Include/ThenInclude, les données devraient être là si elles existent en BDD.
    return immobilisationImmatriculations.Select(ImmobilisationImmatriculationMapper.ToDto);
}

        public async Task<ImmobilisationImmatriculationDto> GetImmobilisationImmatriculationByIdAsync(int id)
        {
            var immobilisationImmatriculation = await _context.ImmobilisationImmatriculations
                // .Include(ip => ip.Immobilisation)
                .Include(ip => ip.EntreeImmobilisation)
                .FirstOrDefaultAsync(ip => ip.IdImmobilisationPropre == id);

            if (immobilisationImmatriculation == null)
            {
                throw new KeyNotFoundException($"Aucune immatriculation d'immobilisation trouvée avec l'ID {id}.");
            }

            return ImmobilisationImmatriculationMapper.ToDto(immobilisationImmatriculation);
        }

        public async Task<ImmobilisationImmatriculationDto> CreateImmobilisationImmatriculationAsync(ImmobilisationImmatriculationDto immobilisationImmatriculationDto)
        {
            if (immobilisationImmatriculationDto == null)
                throw new ArgumentNullException(nameof(immobilisationImmatriculationDto));

            var immobilisationImmatriculation = ImmobilisationImmatriculationMapper.ToEntity(immobilisationImmatriculationDto);
            _context.ImmobilisationImmatriculations.Add(immobilisationImmatriculation);
            await _context.SaveChangesAsync();

            return ImmobilisationImmatriculationMapper.ToDto(immobilisationImmatriculation);
        }
    }
}