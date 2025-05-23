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
                .Include(ip => ip.EntreeImmobilisation)
                    .ThenInclude(ei => ei.Immobilisation)
                .OrderBy(ip => ip.IdImmobilisationPropre)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return immobilisationImmatriculations.Select(ImmobilisationImmatriculationMapper.ToDto).Where(dto => dto != null).Cast<ImmobilisationImmatriculationDto>();
        }

        public async Task<ImmobilisationImmatriculationDto?> GetImmobilisationImmatriculationByIdAsync(int id)
        {
            var immobilisationImmatriculation = await _context.ImmobilisationImmatriculations
                .Include(ip => ip.EntreeImmobilisation)
                .FirstOrDefaultAsync(ip => ip.IdImmobilisationPropre == id);

            if (immobilisationImmatriculation == null)
            {
                return null;
            }

            return ImmobilisationImmatriculationMapper.ToDto(immobilisationImmatriculation);
        }

        public async Task<ImmobilisationImmatriculationDto?> CreateImmobilisationImmatriculationAsync(ImmobilisationImmatriculationDto immobilisationImmatriculationDto)
        {
            if (immobilisationImmatriculationDto == null)
                throw new ArgumentNullException(nameof(immobilisationImmatriculationDto));

            var immobilisationImmatriculation = ImmobilisationImmatriculationMapper.ToEntity(immobilisationImmatriculationDto);
            if (immobilisationImmatriculation == null)
                return null;
            _context.ImmobilisationImmatriculations.Add(immobilisationImmatriculation);
            await _context.SaveChangesAsync();

            return ImmobilisationImmatriculationMapper.ToDto(immobilisationImmatriculation);
        }
    }
}