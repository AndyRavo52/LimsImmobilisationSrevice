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
    public class ReportImmobilisationService : IReportImmobilisationService
    {
        private readonly ImmobilisationContext _context;

        public ReportImmobilisationService(ImmobilisationContext context)
        {
            _context = context;
        }

        public async Task<int> CountReportImmobilisationsAsync()
        {
            return await _context.ReportImmobilisations.CountAsync();
        }

        public async Task<IEnumerable<ReportImmobilisationDto>> GetReportImmobilisationsAsync(int pageIndex, int pageSize)
        {
            var reports = await _context.ReportImmobilisations
                .Include(r => r.Immobilisation)
                .OrderBy(r => r.DateReport)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return reports.Select(ReportImmobilisationMapper.ToDto);
        }

        public async Task<ReportImmobilisationDto> GetReportImmobilisationByIdAsync(int id)
        {
            var report = await _context.ReportImmobilisations
                .Include(r => r.Immobilisation)
                .FirstOrDefaultAsync(r => r.IdReportImmobilisation == id);

            if (report == null)
            {
                throw new Exception("Report d'immobilisation non trouvé");
            }

            return ReportImmobilisationMapper.ToDto(report);
        }

        public async Task<ReportImmobilisationDto> CreateReportImmobilisationAsync(ReportImmobilisationDto dto)
        {
            var entity = ReportImmobilisationMapper.ToEntity(dto);
            _context.ReportImmobilisations.Add(entity);
            await _context.SaveChangesAsync();
            return ReportImmobilisationMapper.ToDto(entity);
        }
    }
}
