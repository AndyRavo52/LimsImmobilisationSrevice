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
    [Route("api/indisponibilites")]
    public class IndisponibiliteController : ControllerBase
    {
        private readonly IIndisponibiliteService _indisponibiliteService;

        // Injection du service via le constructeur
        public IndisponibiliteController(IIndisponibiliteService indisponibiliteService)
        {
            _indisponibiliteService = indisponibiliteService;
        }

        // Récupère le nombre total d'indisponibilités
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalIndisponibilites()
        {
            int total = await _indisponibiliteService.CountIndisponibilitesAsync();
            return Ok(new ApiResponse
            {
                Data = total,
                ViewBag = null,
                IsSuccess = true,
                Message = "Total indisponibilités retrieved successfully.",
                StatusCode = 200
            });
        }

        // Récupère une liste paginée d'indisponibilités
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetIndisponibilites(int position = 1, int pageSize = 10)
        {
            if (position < 1) position = 1; // Position minimale : 1
            if (pageSize < 1) pageSize = 10; // PageSize minimal : 10

            // Récupère les données paginées
            var indisponibilites = await _indisponibiliteService.GetIndisponibilitesAsync(position, pageSize);

            // Calcule les informations de pagination
            int total = await _indisponibiliteService.CountIndisponibilitesAsync();
            var viewBag = new Dictionary<string, object>
            {
                { "nbrPerPage", pageSize },
                { "TotalCount", total },
                { "nbrLinks", (int)Math.Ceiling((double)total / pageSize) },
                { "position", position }
            };

            return Ok(new ApiResponse
            {
                Data = indisponibilites,
                ViewBag = viewBag,
                IsSuccess = true,
                Message = "Indisponibilités retrieved successfully.",
                StatusCode = 200
            });
        }

        // Récupère une indisponibilité par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetIndisponibilite(int id)
        {
            try
            {
                var indisponibilite = await _indisponibiliteService.GetIndisponibiliteByIdAsync(id);
                return Ok(new ApiResponse
                {
                    Data = indisponibilite,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Indisponibilité retrieved successfully.",
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

        // Crée une nouvelle indisponibilité
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateIndisponibilite([FromBody] IndisponibiliteDto indisponibiliteDto)
        {
            var createdIndisponibilite = await _indisponibiliteService.CreateIndisponibiliteAsync(indisponibiliteDto);
            return CreatedAtAction(nameof(GetIndisponibilite), new { id = createdIndisponibilite.IdIndisponibilite }, new ApiResponse
            {
                Data = createdIndisponibilite,
                ViewBag = null,
                IsSuccess = true,
                Message = "Indisponibilité created successfully.",
                StatusCode = 201
            });
        }
        // Dans IndisponibiliteController.cs
[HttpGet("available-immobilisations")]
public async Task<ActionResult<ApiResponse>> GetAvailableImmobilisationsImmatriculees()
{
    var availableImmobilisations = await _indisponibiliteService.GetAvailableImmobilisationsImmatriculeesAsync();
    return Ok(new ApiResponse
    {
        Data = availableImmobilisations,
        ViewBag = null,
        IsSuccess = true,
        Message = "Immobilisations immatriculées disponibles récupérées avec succès.",
        StatusCode = 200
    });
}
    }
}