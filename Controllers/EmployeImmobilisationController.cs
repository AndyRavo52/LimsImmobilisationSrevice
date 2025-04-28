using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Services;
using LimsUtils.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Controllers
{
    [ApiController]
    [Route("api/employes")]
    public class EmployesController : ControllerBase
    {
        private readonly IEmployeService _employeService;

        public EmployesController(IEmployeService employeService)
        {
            _employeService = employeService;
        }

        // Récupère le nombre total d'employés
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalEmployes()
        {
            int total = await _employeService.CountEmployesAsync();
            return Ok(new ApiResponse
            {
                Data = total,
                ViewBag = null,
                IsSuccess = true,
                Message = "Total des employés récupéré avec succès.",
                StatusCode = 200
            });
        }

        // Récupère une liste paginée d'employés
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetEmployes(int position = 1, int pageSize = 10)
        {
            if (position < 1) position = 1;
            if (pageSize < 1) pageSize = 10;

            var employes = await _employeService.GetEmployesAsync(position, pageSize);
            int total = await _employeService.CountEmployesAsync();

            var viewBag = new Dictionary<string, object>
            {
                { "nbrPerPage", pageSize },
                { "TotalCount", total },
                { "nbrLinks", (int)Math.Ceiling((double)total / pageSize) },
                { "position", position }
            };

            return Ok(new ApiResponse
            {
                Data = employes,
                ViewBag = viewBag,
                IsSuccess = true,
                Message = "Liste des employés récupérée avec succès.",
                StatusCode = 200
            });
        }

        // Recherche d'employés
        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse>> SearchEmployes([FromQuery] string searchTerm = "")
        {
            var results = await _employeService.SearchEmployesAsync(searchTerm);
            return Ok(new ApiResponse
            {
                Data = results,
                ViewBag = null,
                IsSuccess = true,
                Message = "Recherche des employés effectuée avec succès.",
                StatusCode = 200
            });
        }

        // Récupère un employé par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetEmploye(int id)
        {
            try
            {
                var employe = await _employeService.GetEmployeByIdAsync(id);
                return Ok(new ApiResponse
                {
                    Data = employe,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Employé récupéré avec succès.",
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = 404
                });
            }
        }
    }
}
