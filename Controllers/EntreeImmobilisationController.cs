using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Services;
using LimsUtils.Api; // Supposé existant, comme dans l'exemple
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Controllers
{
    [ApiController]
    [Route("api/entree-immobilisations")]
    public class EntreeImmobilisationController : ControllerBase
    {
        private readonly IEntreeImmobilisationService _entreeImmobilisationService;

        // Injection du service via le constructeur
        public EntreeImmobilisationController(IEntreeImmobilisationService entreeImmobilisationService)
        {
            _entreeImmobilisationService = entreeImmobilisationService;
        }

        // Récupère le nombre total d'entrées d'immobilisations
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalEntreeImmobilisations()
        {
            int total = await _entreeImmobilisationService.CountEntreeImmobilisationsAsync();
            return Ok(new ApiResponse
            {
                Data = total,
                ViewBag = null,
                IsSuccess = true,
                Message = "Total des entrées d'immobilisations récupéré avec succès.",
                StatusCode = 200
            });
        }

        // Récupère une liste paginée d'entrées d'immobilisations
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetEntreeImmobilisations(int position = 1, int pageSize = 5)
        {
            if (position < 1) position = 1;
            if (pageSize < 1) pageSize = 5;

            var entreeImmobilisations = await _entreeImmobilisationService.GetEntreeImmobilisationsAsync(position, pageSize);
            int total = await _entreeImmobilisationService.CountEntreeImmobilisationsAsync();
            var viewBag = new Dictionary<string, object>
            {
                { "nbrPerPage", pageSize },
                { "TotalCount", total },
                { "nbrLinks", (int)Math.Ceiling((double)total / pageSize) },
                { "position", position }
            };

            return Ok(new ApiResponse
            {
                Data = entreeImmobilisations,
                ViewBag = viewBag,
                IsSuccess = true,
                Message = "Entrées d'immobilisations récupérées avec succès.",
                StatusCode = 200
            });
        }

        // Récupère une entrée d'immobilisation par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetEntreeImmobilisation(int id)
        {
            try
            {
                var entreeImmobilisation = await _entreeImmobilisationService.GetEntreeImmobilisationByIdAsync(id);
                return Ok(new ApiResponse
                {
                    Data = entreeImmobilisation,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Entrée d'immobilisation récupérée avec succès.",
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
                    Message = ex.Message, // Utilise le message de l'exception (ex: "Entrée immobilisation non trouvée")
                    StatusCode = 404
                });
            }
        }

        // Crée une nouvelle entrée d'immobilisation
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateEntreeImmobilisation([FromBody] EntreeImmobilisationDto entreeImmobilisationDto)
        {
            var createdEntreeImmobilisation = await _entreeImmobilisationService.CreateEntreeImmobilisationAsync(entreeImmobilisationDto);
            return CreatedAtAction(nameof(GetEntreeImmobilisation), new { id = createdEntreeImmobilisation.IdEntreeImmobilisation }, new ApiResponse
            {
                Data = createdEntreeImmobilisation,
                ViewBag = null,
                IsSuccess = true,
                Message = "Entrée d'immobilisation créée avec succès.",
                StatusCode = 201
            });
        }


        [HttpGet("non-immatriculees")]
public async Task<ActionResult<ApiResponse>> GetEntreeImmobilisationsNonImmatriculees()
{
    var entreeImmobilisations = await _entreeImmobilisationService.GetEntreeImmobilisationsNonImmatriculeesAsync();
    return Ok(new ApiResponse
    {
        Data = entreeImmobilisations,
        ViewBag = null,
        IsSuccess = true,
        Message = "Entrées d'immobilisations non immatriculées récupérées avec succès.",
        StatusCode = 200
    });
}
    }
}