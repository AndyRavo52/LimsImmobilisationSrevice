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
    [Route("api/objets-indisponibilite")]
    public class ObjetIndisponibiliteController : ControllerBase
    {
        private readonly IObjetIndisponibiliteService _objetIndisponibiliteService;

        // Injection du service via le constructeur
        public ObjetIndisponibiliteController(IObjetIndisponibiliteService objetIndisponibiliteService)
        {
            _objetIndisponibiliteService = objetIndisponibiliteService;
        }

        // Récupère le nombre total d'objets d'indisponibilité
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalObjetsIndisponibilite()
        {
            int total = await _objetIndisponibiliteService.CountObjetsIndisponibiliteAsync();
            return Ok(new ApiResponse
            {
                Data = total,
                ViewBag = null,
                IsSuccess = true,
                Message = "Total objets d'indisponibilité retrieved successfully.",
                StatusCode = 200
            });
        }

        // Récupère une liste paginée d'objets d'indisponibilité
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetObjetsIndisponibilite(int position = 1, int pageSize = 10)
        {
            if (position < 1) position = 1; // Position minimale : 1
            if (pageSize < 1) pageSize = 10; // Taille de page minimale : 1, valeur par défaut : 10

            // Récupère les données paginées
            var objetsIndisponibilite = await _objetIndisponibiliteService.GetObjetsIndisponibiliteAsync(position, pageSize);

            // Calcule les informations de pagination
            int total = await _objetIndisponibiliteService.CountObjetsIndisponibiliteAsync();
            var viewBag = new Dictionary<string, object>
            {
                { "nbrPerPage", pageSize },
                { "TotalCount", total },
                { "nbrLinks", (int)Math.Ceiling((double)total / pageSize) },
                { "position", position }
            };

            return Ok(new ApiResponse
            {
                Data = objetsIndisponibilite,
                ViewBag = viewBag,
                IsSuccess = true,
                Message = "Objets d'indisponibilité retrieved successfully.",
                StatusCode = 200
            });
        }

        // Récupère un objet d'indisponibilité par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetObjetIndisponibilite(int id)
        {
            try
            {
                var objetIndisponibilite = await _objetIndisponibiliteService.GetObjetIndisponibiliteByIdAsync(id);
                return Ok(new ApiResponse
                {
                    Data = objetIndisponibilite,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Objet d'indisponibilité retrieved successfully.",
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

        // Crée un nouvel objet d'indisponibilité
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateObjetIndisponibilite([FromBody] ObjetIndisponibiliteDto objetIndisponibiliteDto)
        {
            var createdObjet = await _objetIndisponibiliteService.CreateObjetIndisponibiliteAsync(objetIndisponibiliteDto);
            if (createdObjet == null)
            {
                return BadRequest(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Erreur lors de la création de l'objet d'indisponibilité.",
                    StatusCode = 400
                });
            }
            return CreatedAtAction(nameof(GetObjetIndisponibilite), new { id = createdObjet.IdObjetIndisponibilite }, new ApiResponse
            {
                Data = createdObjet,
                ViewBag = null,
                IsSuccess = true,
                Message = "Objet d'indisponibilité created successfully.",
                StatusCode = 201
            });
        }

        // Met à jour un objet d'indisponibilité existant
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateObjetIndisponibilite(int id, [FromBody] ObjetIndisponibiliteDto objetIndisponibiliteDto)
        {
            if (id != objetIndisponibiliteDto.IdObjetIndisponibilite)
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

            try
            {
                var updatedObjet = await _objetIndisponibiliteService.UpdateObjetIndisponibiliteAsync(id, objetIndisponibiliteDto);
                return Ok(new ApiResponse
                {
                    Data = updatedObjet,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Objet d'indisponibilité updated successfully.",
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

        // Supprime un objet d'indisponibilité
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteObjetIndisponibilite(int id)
        {
            var result = await _objetIndisponibiliteService.DeleteObjetIndisponibiliteAsync(id);
            if (!result)
            {
                return NotFound(new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = "Objet d'indisponibilité not found.",
                    StatusCode = 404
                });
            }

            return Ok(new ApiResponse
            {
                Data = null,
                ViewBag = null,
                IsSuccess = true,
                Message = "Objet d'indisponibilité deleted successfully.",
                StatusCode = 200
            });
        }
    }
}