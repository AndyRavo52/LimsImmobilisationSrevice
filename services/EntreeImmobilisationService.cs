using LimsImmobilisationService.Data;
using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Mappers;
using Microsoft.EntityFrameworkCore;


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

            return entreeImmobilisations.Select(EntreeImmobilisationMapper.ToDto);
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

        public async Task<IEnumerable<EntreeImmobilisationDto>> GetEntreeImmobilisationsNonImmatriculeesAsync()
{
    var immatriculeesIds = await _context.ImmobilisationImmatriculations
        .Select(ip => ip.IdEntreeImmobilisation)
        .ToListAsync();

    var entreeImmobilisations = await _context.EntreeImmobilisations
        .Where(ei => !immatriculeesIds.Contains(ei.IdEntreeImmobilisation))
        .Include(ei => ei.Immobilisation)
        .ToListAsync();

    return entreeImmobilisations.Select(EntreeImmobilisationMapper.ToDto);
}


    }
}