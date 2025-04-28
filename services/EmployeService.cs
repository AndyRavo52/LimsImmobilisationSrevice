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
    public class EmployeService : IEmployeService
    {
        private readonly ImmobilisationContext _context;

        // Constructeur : Injection de dépendance du DbContext
        public EmployeService(ImmobilisationContext context)
        {
            _context = context;
        }

        // Compte le nombre total d'employés
        public async Task<int> CountEmployesAsync()
        {
            return await _context.Employes.CountAsync();
        }

        // Récupère une liste paginée d'employés
        public async Task<IEnumerable<EmployeDto>> GetEmployesAsync(int pageIndex, int pageSize)
        {
            var employes = await _context.Employes
                .OrderBy(e => e.Nom)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return employes.Select(EmployeMapper.ToDto);
        }

        // Récupère un employé par son ID
        public async Task<EmployeDto> GetEmployeByIdAsync(int id)
        {
            var employe = await _context.Employes
                .FirstOrDefaultAsync(e => e.IdEmploye == id);

            if (employe == null)
                throw new Exception("Employé non trouvé");

            return EmployeMapper.ToDto(employe);
        }

        // Recherche des employés en fonction d'un terme (sur le nom ou le matricule)
        public async Task<IEnumerable<EmployeDto>> SearchEmployesAsync(string searchTerm)
        {
            var query = _context.Employes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e =>
                    EF.Functions.Like(e.Nom,      $"%{searchTerm}%") ||
                    EF.Functions.Like(e.Matricule, $"%{searchTerm}%"));
            }

            var results = await query
                .OrderBy(e => e.Nom)
                .ToListAsync();

            return results.Select(EmployeMapper.ToDto);
        }
    }
}
