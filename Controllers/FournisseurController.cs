using LimsImmobilisationService.Dtos;
using LimsImmobilisationService.Services;
using LimsUtils.Api;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LimsImmobilisationService.Controllers
{
    [ApiController]
    [Route("api/fournisseurs")] // Chemin de l'API pour les fournisseurs
    public class FournisseurController : ControllerBase
    {
        private readonly IFournisseurService _fournisseurService;

        // Injection du service via le constructeur
        public FournisseurController(IFournisseurService fournisseurService)
        {
            _fournisseurService = fournisseurService;
        }

        // Récupère le nombre total de fournisseurs
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalFournisseurs()
        {
            int total = await _fournisseurService.CountFournisseursAsync();
            return Ok(new ApiResponse
            {
                Data = total,
                ViewBag = null,
                IsSuccess = true,
                Message = "Total fournisseurs retrieved successfully.",
                StatusCode = 200
            });
        }

        // Récupère une liste paginée de fournisseurs
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetFournisseurs(int position = 1, int pageSize = 10)
        {
            if (position < 1) position = 1; // Position minimale : 1
            if (pageSize < 1) pageSize = 10; // Taille de page minimale : 1, valeur par défaut : 10

            // Récupère les données paginées
            var fournisseurs = await _fournisseurService.GetFournisseursAsync(position, pageSize);

            // Calcule les informations de pagination
            int total = await _fournisseurService.CountFournisseursAsync();
            var viewBag = new Dictionary<string, object>
            {
                { "nbrPerPage", pageSize },
                { "TotalCount", total },
                { "nbrLinks", (int)Math.Ceiling((double)total / pageSize) },
                { "position", position }
            };

            return Ok(new ApiResponse
            {
                Data = fournisseurs,
                ViewBag = viewBag,
                IsSuccess = true,
                Message = "Fournisseurs retrieved successfully.",
                StatusCode = 200
            });
        }

        // Récupère un fournisseur par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetFournisseur(int id)
        {
            var fournisseur = await _fournisseurService.GetFournisseurByIdAsync(id);
            if (fournisseur == null)
            {
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Fournisseur not found.",
                    StatusCode = 404
                });
            }

            return Ok(new ApiResponse
            {
                Data = fournisseur,
                ViewBag = null,
                IsSuccess = true,
                Message = "Fournisseur retrieved successfully.",
                StatusCode = 200
            });
        }

        // Crée un nouveau fournisseur
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateFournisseur([FromBody] FournisseurDto fournisseurDto)
        {
            var createdFournisseur = await _fournisseurService.CreateFournisseurAsync(fournisseurDto);
            return CreatedAtAction(nameof(GetFournisseur), new { id = createdFournisseur.IdFournisseur }, new ApiResponse
            {
                Data = createdFournisseur,
                ViewBag = null,
                IsSuccess = true,
                Message = "Fournisseur created successfully.",
                StatusCode = 201
            });
        }

        // Met à jour un fournisseur existant
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateFournisseur(int id, [FromBody] FournisseurDto fournisseurDto)
        {
            if (id != fournisseurDto.IdFournisseur)
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

            var updatedFournisseur = await _fournisseurService.UpdateFournisseurAsync(id, fournisseurDto);
            if (updatedFournisseur == null)
            {
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Fournisseur not found.",
                    StatusCode = 404
                });
            }

            return Ok(new ApiResponse
            {
                Data = updatedFournisseur,
                ViewBag = null,
                IsSuccess = true,
                Message = "Fournisseur updated successfully.",
                StatusCode = 200
            });
        }

        // Supprime un fournisseur
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteFournisseur(int id)
        {
            bool result = await _fournisseurService.DeleteFournisseurAsync(id);
            if (!result)
            {
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Fournisseur not found.",
                    StatusCode = 404
                });
            }

            return Ok(new ApiResponse
            {
                Data = null,
                ViewBag = null,
                IsSuccess = true,
                Message = "Fournisseur deleted successfully.",
                StatusCode = 200
            });
        }
    }
}