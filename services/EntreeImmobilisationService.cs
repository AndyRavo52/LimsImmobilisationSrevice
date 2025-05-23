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
    public class EntreeImmobilisationService : IEntreeImmobilisationService
    {
        private readonly ImmobilisationContext _context;

        public EntreeImmobilisationService(ImmobilisationContext context)
        {
            _context = context;
        }

        public async Task<int> CountEntreeImmobilisationsAsync()
        {
            return await _context.EntreeImmobilisations.CountAsync();
        }

        public async Task<IEnumerable<EntreeImmobilisationDto>> GetEntreeImmobilisationsAsync(int pageIndex, int pageSize)
        {
            var entreeImmobilisations = await _context.EntreeImmobilisations
                .Include(ei => ei.Immobilisation)
                    .ThenInclude(i => i.Marque)
                .Include(ei => ei.Fournisseur)
                .OrderByDescending(ei => ei.DateEntree)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return entreeImmobilisations.Select(EntreeImmobilisationMapper.ToDto).Where(dto => dto != null).Cast<EntreeImmobilisationDto>();
        }

        public async Task<EntreeImmobilisationDto> GetEntreeImmobilisationByIdAsync(int id)
        {
            var entreeImmobilisation = await _context.EntreeImmobilisations
                .Include(ei => ei.Immobilisation)
                .Include(ei => ei.Fournisseur)
                .FirstOrDefaultAsync(ei => ei.IdEntreeImmobilisation == id);

            if (entreeImmobilisation == null)
            {
                throw new Exception("Entrée immobilisation non trouvée");
            }

            return EntreeImmobilisationMapper.ToDto(entreeImmobilisation);
        }

        public async Task<EntreeImmobilisationDto> CreateEntreeImmobilisationAsync(EntreeImmobilisationDto entreeImmobilisationDto)
        {
            var entreeImmobilisation = EntreeImmobilisationMapper.ToEntity(entreeImmobilisationDto);
            _context.EntreeImmobilisations.Add(entreeImmobilisation);
            await _context.SaveChangesAsync();

            return EntreeImmobilisationMapper.ToDto(entreeImmobilisation);
        }

        // public async Task<IEnumerable<EntreeImmobilisationDto>> GetEntreeImmobilisationsNonImmatriculeesAsync()
        // {
        //     var immatriculeesIds = await _context.ImmobilisationImmatriculations
        //         .Select(ip => ip.IdEntreeImmobilisation)
        //         .ToListAsync();

        //     var entreeImmobilisations = await _context.EntreeImmobilisations
        //         .Where(ei => !immatriculeesIds.Contains(ei.IdEntreeImmobilisation))
        //         .Include(ei => ei.Immobilisation)
        //         .ToListAsync();

        //     return entreeImmobilisations.Select(EntreeImmobilisationMapper.ToDto);
        // }
        public async Task<IEnumerable<EntreeImmobilisationAvecResteDto>> GetEntreeImmobilisationsAvecImmatriculationsRestantesAsync()
{
    var entreeImmobilisationsAvecReste = await _context.EntreeImmobilisations
        .Include(ei => ei.Immobilisation)
        .GroupJoin(
            _context.ImmobilisationImmatriculations,
            ei => ei.IdEntreeImmobilisation,
            ip => ip.IdEntreeImmobilisation,
            (ei, ips) => new { EntreeImmobilisation = ei, CountImmatriculations = ips.Count() }
        )
        .Where(x => x.EntreeImmobilisation.Quantite > x.CountImmatriculations)
        .Select(x => new EntreeImmobilisationAvecResteDto
        {
            IdEntreeImmobilisation = x.EntreeImmobilisation.IdEntreeImmobilisation,
            Quantite = x.EntreeImmobilisation.Quantite,
            PrixAchat = x.EntreeImmobilisation.PrixAchat,
            DateEntree = x.EntreeImmobilisation.DateEntree,
            BonReception = x.EntreeImmobilisation.BonReception,
            BonDeCommande = x.EntreeImmobilisation.BonDeCommande,
            NumeroFacture = x.EntreeImmobilisation.NumeroFacture,
            IdFournisseur = x.EntreeImmobilisation.IdFournisseur,
            Fournisseur = x.EntreeImmobilisation.Fournisseur != null ? FournisseurMapper.ToDto(x.EntreeImmobilisation.Fournisseur) : null,
            IdImmobilisation = x.EntreeImmobilisation.IdImmobilisation,
            Immobilisation = x.EntreeImmobilisation.Immobilisation != null ? ImmobilisationMapper.ToDto(x.EntreeImmobilisation.Immobilisation) : null,
            ResteImmatriculations = x.EntreeImmobilisation.Quantite.HasValue ? (x.EntreeImmobilisation.Quantite.Value - x.CountImmatriculations) : 0
        })
        .ToListAsync();

    return entreeImmobilisationsAvecReste;
}

        public async Task<Dictionary<string, decimal>> GetDepensesParMoisAsync(int annee)
        {
            return await _context.EntreeImmobilisations
                .Where(ei => ei.DateEntree.HasValue && ei.DateEntree.Value.Year == annee)
                .GroupBy(ei => new { Year = ei.DateEntree!.Value.Year, Month = ei.DateEntree!.Value.Month })
                .Select(g => new
                {
                    Periode = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Total = g.Sum(ei => ei.PrixAchat ?? 0)
                })
                .ToDictionaryAsync(x => x.Periode, x => x.Total);
        }
    }
}