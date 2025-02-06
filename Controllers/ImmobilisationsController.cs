using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Services;
using LimsUtils.Api;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Controllers
{
    [ApiController]
    [Route("api/immobilisations")]
    public class ImmobilisationsController : ControllerBase
    {
        private readonly IImmobilisationService _immobilisationService;

        // Injection du service via le constructeur
        public ImmobilisationsController(IImmobilisationService immobilisationService)
        {
            _immobilisationService = immobilisationService;
        }

        // Récupère le nombre total d'immobilisations
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalImmobilisations()
        {
            int total = await _immobilisationService.CountImmobilisationsAsync();
            return Ok(new ApiResponse
            {
                Data = total,
                ViewBag = null,
                IsSuccess = true,
                Message = "Total immobilisations retrieved successfully.",
                StatusCode = 200
            });
        }

        // Récupère une liste paginée d'immobilisations
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetImmobilisations(int position = 1, int pageSize = 10)
        {
            if (position < 1) position = 1; // Position minimale : 1
            if (pageSize < 1) pageSize = 10; // PageSize minimal : 1, valeur par défaut : 10

            // Récupère les données paginées
            var immobilisations = await _immobilisationService.GetImmobilisationsAsync(position, pageSize);

            // Calcule les informations de pagination
            int total = await _immobilisationService.CountImmobilisationsAsync();
            var viewBag = new Dictionary<string, object>
            {
                { "nbrPerPage", pageSize },
                { "TotalCount", total },
                { "nbrLinks", (int)Math.Ceiling((double)total / pageSize) },
                { "position", position }
            };

            return Ok(new ApiResponse
            {
                Data = immobilisations,
                ViewBag = viewBag,
                IsSuccess = true,
                Message = "Immobilisations retrieved successfully.",
                StatusCode = 200
            });
        }

        // Récupère une immobilisation par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetImmobilisation(int id)
        {
            var immobilisation = await _immobilisationService.GetImmobilisationByIdAsync(id);
            if (immobilisation == null)
            {
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Immobilisation not found.",
                    StatusCode = 404
                });
            }

            return Ok(new ApiResponse
            {
                Data = immobilisation,
                ViewBag = null,
                IsSuccess = true,
                Message = "Immobilisation retrieved successfully.",
                StatusCode = 200
            });
        }

        // Crée une nouvelle immobilisation
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateImmobilisation([FromBody] ImmobilisationDto immobilisationDto)
        {
            var createdImmobilisation = await _immobilisationService.CreateImmobilisationAsync(immobilisationDto);
            return CreatedAtAction(nameof(GetImmobilisation), new { id = createdImmobilisation.IdImmobilisation }, new ApiResponse
            {
                Data = createdImmobilisation,
                ViewBag = null,
                IsSuccess = true,
                Message = "Immobilisation created successfully.",
                StatusCode = 201
            });
        }

        // Met à jour une immobilisation existante
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateImmobilisation(int id, [FromBody] ImmobilisationDto immobilisationDto)
        {
            if (id != immobilisationDto.IdImmobilisation)
            {
                return BadRequest(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "ID mismatch.",
                    StatusCode = 400
                });
            }

            var updatedImmobilisation = await _immobilisationService.UpdateImmobilisationAsync(id, immobilisationDto);
            return Ok(new ApiResponse
            {
                Data = updatedImmobilisation,
                ViewBag = null,
                IsSuccess = true,
                Message = "Immobilisation updated successfully.",
                StatusCode = 200
            });
        }

        // Supprime une immobilisation
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteImmobilisation(int id)
        {
            bool result = await _immobilisationService.DeleteImmobilisationAsync(id);
            if (!result)
            {
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Immobilisation not found.",
                    StatusCode = 404
                });
            }

            return NoContent();
        }
    }
}