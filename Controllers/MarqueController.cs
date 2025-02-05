using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Services;
using LimsImmobilisationService.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Controllers
{
    [ApiController]
    [Route("api/marques")]
    public class MarquesController : ControllerBase
    {
        private readonly IMarqueService _marqueService;

        // Injection du service via le constructeur
        public MarquesController(IMarqueService marqueService)
        {
            _marqueService = marqueService;
        }

        // Récupère le nombre total de marques
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalMarques()
        {
            int total = await _marqueService.CountMarquesAsync();
            return Ok(new ApiResponse
            {
                Data = total,
                ViewBag = null,
                IsSuccess = true,
                Message = "Total marques retrieved successfully.",
                StatusCode = 200
            });
        }

        // Récupère une liste paginée de marques
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetMarques(int position = 1, int pageSize = 10)
        {
            if (position < 1) position = 1; // Position minimale : 1
            if (pageSize < 1) pageSize = 10; // Taille de page minimale : 1, valeur par défaut : 10

            // Récupère les données paginées
            var marques = await _marqueService.GetMarquesAsync(position, pageSize);

            // Calcule les informations de pagination
            int total = await _marqueService.CountMarquesAsync();
            var viewBag = new Dictionary<string, object>
            {
                { "nbrPerPage", pageSize },
                { "TotalCount", total },
                { "nbrLinks", (int)Math.Ceiling((double)total / pageSize) },
                { "position", position }
            };

            return Ok(new ApiResponse
            {
                Data = marques,
                ViewBag = viewBag,
                IsSuccess = true,
                Message = "Marques retrieved successfully.",
                StatusCode = 200
            });
        }

        // Récupère une marque par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetMarque(int id)
        {
            var marque = await _marqueService.GetMarqueByIdAsync(id);
            if (marque == null)
            {
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Marque not found.",
                    StatusCode = 404
                });
            }

            return Ok(new ApiResponse
            {
                Data = marque,
                ViewBag = null,
                IsSuccess = true,
                Message = "Marque retrieved successfully.",
                StatusCode = 200
            });
        }

        // Crée une nouvelle marque
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateMarque([FromBody] MarqueDto marqueDto)
        {
            var createdMarque = await _marqueService.CreateMarqueAsync(marqueDto);
            return CreatedAtAction(nameof(GetMarque), new { id = createdMarque.IdMarque }, new ApiResponse
            {
                Data = createdMarque,
                ViewBag = null,
                IsSuccess = true,
                Message = "Marque created successfully.",
                StatusCode = 201
            });
        }

        // Met à jour une marque existante
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateMarque(int id, [FromBody] MarqueDto marqueDto)
        {
            if (id != marqueDto.IdMarque)
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

            var updatedMarque = await _marqueService.UpdateMarqueAsync(id, marqueDto);
            if (updatedMarque == null)
            {
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Marque not found.",
                    StatusCode = 404
                });
            }

            return Ok(new ApiResponse
            {
                Data = updatedMarque,
                ViewBag = null,
                IsSuccess = true,
                Message = "Marque updated successfully.",
                StatusCode = 200
            });
        }

        // Supprime une marque
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteMarque(int id)
        {
            bool result = await _marqueService.DeleteMarqueAsync(id);
            if (!result)
            {
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Marque not found.",
                    StatusCode = 404
                });
            }

            return Ok(new ApiResponse
            {
                Data = null,
                ViewBag = null,
                IsSuccess = true,
                Message = "Marque deleted successfully.",
                StatusCode = 200
            });
        }
    }
}

