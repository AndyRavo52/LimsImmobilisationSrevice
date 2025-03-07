using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Services;
using LimsUtils.Api;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Controllers
{
    [ApiController]
    [Route("api/localisations")] // Chemin de l'API pour les localisations
    public class LocalisationController : ControllerBase
    {
        private readonly ILocalisationService _localisationService;

        // Injection du service via le constructeur
        public LocalisationController(ILocalisationService localisationService)
        {
            _localisationService = localisationService;
        }

        // Récupère le nombre total de localisations
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalLocalisations()
        {
            int total = await _localisationService.CountLocalisationsAsync();
            return Ok(new ApiResponse
            {
                Data = total,
                ViewBag = null,
                IsSuccess = true,
                Message = "Total localisations retrieved successfully.",
                StatusCode = 200
            });
        }

        // Récupère une liste paginée de localisations
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetLocalisations(int position = 1, int pageSize = 10)
        {
            if (position < 1) position = 1; // Position minimale : 1
            if (pageSize < 1) pageSize = 10; // Taille de page minimale : 1, valeur par défaut : 10

            // Récupère les données paginées
            var localisations = await _localisationService.GetLocalisationsAsync(position, pageSize);

            // Calcule les informations de pagination
            int total = await _localisationService.CountLocalisationsAsync();
            var viewBag = new Dictionary<string, object>
            {
                { "nbrPerPage", pageSize },
                { "TotalCount", total },
                { "nbrLinks", (int)Math.Ceiling((double)total / pageSize) },
                { "position", position }
            };

            return Ok(new ApiResponse
            {
                Data = localisations,
                ViewBag = viewBag,
                IsSuccess = true,
                Message = "Localisations retrieved successfully.",
                StatusCode = 200
            });
        }

        // Récupère une localisation par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetLocalisation(int id)
        {
            var localisation = await _localisationService.GetLocalisationByIdAsync(id);
            if (localisation == null)
            {
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Localisation not found.",
                    StatusCode = 404
                });
            }

            return Ok(new ApiResponse
            {
                Data = localisation,
                ViewBag = null,
                IsSuccess = true,
                Message = "Localisation retrieved successfully.",
                StatusCode = 200
            });
        }

        // Crée une nouvelle localisation
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateLocalisation([FromBody] LocalisationDto localisationDto)
        {
            var createdLocalisation = await _localisationService.CreateLocalisationAsync(localisationDto);
            return CreatedAtAction(nameof(GetLocalisation), new { id = createdLocalisation.IdLocalisation }, new ApiResponse
            {
                Data = createdLocalisation,
                ViewBag = null,
                IsSuccess = true,
                Message = "Localisation created successfully.",
                StatusCode = 201
            });
        }

        // Met à jour une localisation existante
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateLocalisation(int id, [FromBody] LocalisationDto localisationDto)
        {
            if (id != localisationDto.IdLocalisation)
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

            var updatedLocalisation = await _localisationService.UpdateLocalisationAsync(id, localisationDto);
            if (updatedLocalisation == null)
            {
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Localisation not found.",
                    StatusCode = 404
                });
            }

            return Ok(new ApiResponse
            {
                Data = updatedLocalisation,
                ViewBag = null,
                IsSuccess = true,
                Message = "Localisation updated successfully.",
                StatusCode = 200
            });
        }

        // Supprime une localisation
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteLocalisation(int id)
        {
            bool result = await _localisationService.DeleteLocalisationAsync(id);
            if (!result)
            {
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Localisation not found.",
                    StatusCode = 404
                });
            }

            return Ok(new ApiResponse
            {
                Data = null,
                ViewBag = null,
                IsSuccess = true,
                Message = "Localisation deleted successfully.",
                StatusCode = 200
            });
        }
    }
}