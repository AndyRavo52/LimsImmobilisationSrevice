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
    [Route("api/immobilisation-immatriculations")]
    public class ImmobilisationImmatriculationController : ControllerBase
    {
        private readonly IImmobilisationImmatriculationService _immobilisationImmatriculationService;

        // Injection du service via le constructeur
        public ImmobilisationImmatriculationController(IImmobilisationImmatriculationService immobilisationImmatriculationService)
        {
            _immobilisationImmatriculationService = immobilisationImmatriculationService;
        }

        // Récupère le nombre total d'immatriculations d'immobilisations
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalImmobilisationImmatriculations()
        {
            int total = await _immobilisationImmatriculationService.CountImmobilisationImmatriculationsAsync();
            return Ok(new ApiResponse
            {
                Data = total,
                ViewBag = null,
                IsSuccess = true,
                Message = "Total des immatriculations d'immobilisations récupéré avec succès.",
                StatusCode = 200
            });
        }

        // Récupère une liste paginée d'immatriculations d'immobilisations
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetImmobilisationImmatriculations(int position = 1, int pageSize = 5)
        {
            if (position < 1) position = 1;
            if (pageSize < 1) pageSize = 5;

            var immobilisationImmatriculations = await _immobilisationImmatriculationService.GetImmobilisationImmatriculationsAsync(position, pageSize);
            int total = await _immobilisationImmatriculationService.CountImmobilisationImmatriculationsAsync();
            var viewBag = new Dictionary<string, object>
            {
                { "nbrPerPage", pageSize },
                { "TotalCount", total },
                { "nbrLinks", (int)Math.Ceiling((double)total / pageSize) },
                { "position", position }
            };

            return Ok(new ApiResponse
            {
                Data = immobilisationImmatriculations,
                ViewBag = viewBag,
                IsSuccess = true,
                Message = "Immatriculations d'immobilisations récupérées avec succès.",
                StatusCode = 200
            });
        }

        // Récupère une immatriculation d'immobilisation par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetImmobilisationImmatriculation(int id)
        {
            try
            {
                var immobilisationImmatriculation = await _immobilisationImmatriculationService.GetImmobilisationImmatriculationByIdAsync(id);
                return Ok(new ApiResponse
                {
                    Data = immobilisationImmatriculation,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Immatriculation d'immobilisation récupérée avec succès.",
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
                    Message = ex.Message, // Par exemple : "Aucune immatriculation d'immobilisation trouvée avec l'ID {id}."
                    StatusCode = 404
                });
            }
        }

        // Crée une nouvelle immatriculation d'immobilisation
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateImmobilisationImmatriculation([FromBody] ImmobilisationImmatriculationDto immobilisationImmatriculationDto)
        {
            var createdImmobilisationImmatriculation = await _immobilisationImmatriculationService.CreateImmobilisationImmatriculationAsync(immobilisationImmatriculationDto);
            if (createdImmobilisationImmatriculation == null)
            {
                return BadRequest(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Erreur lors de la création de l'immatriculation d'immobilisation.",
                    StatusCode = 400
                });
            }
            return CreatedAtAction(nameof(GetImmobilisationImmatriculation), new { id = createdImmobilisationImmatriculation.IdImmobilisationPropre }, new ApiResponse
            {
                Data = createdImmobilisationImmatriculation,
                ViewBag = null,
                IsSuccess = true,
                Message = "Immatriculation d'immobilisation créée avec succès.",
                StatusCode = 201
            });
        }
    }
}