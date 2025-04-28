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
    [Route("api/assignations")]
    public class AssignationController : ControllerBase
    {
        private readonly IAssignationService _assignationService;

        public AssignationController(IAssignationService assignationService)
        {
            _assignationService = assignationService;
        }

        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalAssignations()
        {
            int total = await _assignationService.CountAssignationsAsync();
            return Ok(new ApiResponse
            {
                Data = total,
                ViewBag = null,
                IsSuccess = true,
                Message = "Total assignations retrieved successfully.",
                StatusCode = 200
            });
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAssignations(int position = 1, int pageSize = 10)
        {
            if (position < 1) position = 1;
            if (pageSize < 1) pageSize = 10;

            var assignations = await _assignationService.GetAssignationsAsync(position, pageSize);
            int total = await _assignationService.CountAssignationsAsync();

            var viewBag = new Dictionary<string, object>
            {
                { "nbrPerPage", pageSize },
                { "TotalCount", total },
                { "nbrLinks", (int)Math.Ceiling((double)total / pageSize) },
                { "position", position }
            };

            return Ok(new ApiResponse
            {
                Data = assignations,
                ViewBag = viewBag,
                IsSuccess = true,
                Message = "Assignations retrieved successfully.",
                StatusCode = 200
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetAssignation(int id)
        {
            try
            {
                var assignation = await _assignationService.GetAssignationByIdAsync(id);
                return Ok(new ApiResponse
                {
                    Data = assignation,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Assignation retrieved successfully.",
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

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateAssignation([FromBody] AssignationDto assignationDto)
        {
            var createdAssignation = await _assignationService.CreateAssignationAsync(assignationDto);
            return CreatedAtAction(nameof(GetAssignation), new { id = createdAssignation.IdAssignation }, new ApiResponse
            {
                Data = createdAssignation,
                ViewBag = null,
                IsSuccess = true,
                Message = "Assignation created successfully.",
                StatusCode = 201
            });
        }

        [HttpGet("employe/{matricule}")]
        public async Task<ActionResult<ApiResponse>> GetEmployeByMatricule(string matricule)
        {
            try
            {
                var employe = await _assignationService.GetEmployeByMatriculeAsync(matricule);
                return Ok(new ApiResponse
                {
                    Data = employe,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Employé retrieved successfully.",
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

        [HttpGet("available-immobilisations")]
        public async Task<ActionResult<ApiResponse>> GetAvailableImmobilisations()
        {
            var availableImmobilisations = await _assignationService.GetAvailableImmobilisationsAsync();
            return Ok(new ApiResponse
            {
                Data = availableImmobilisations,
                ViewBag = null,
                IsSuccess = true,
                Message = "Available immobilisations retrieved successfully.",
                StatusCode = 200
            });
        }

        [HttpGet("localisations")]
        public async Task<ActionResult<ApiResponse>> GetLocalisations()
        {
            var localisations = await _assignationService.GetLocalisationsAsync();
            return Ok(new ApiResponse
            {
                Data = localisations,
                ViewBag = null,
                IsSuccess = true,
                Message = "Localisations retrieved successfully.",
                StatusCode = 200
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateAssignation(int id, [FromBody] AssignationDto assignationDto)
        {
            try
            {
                var updatedAssignation = await _assignationService.UpdateAssignationAsync(id, assignationDto);
                return Ok(new ApiResponse
                {
                    Data = updatedAssignation,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Assignation updated successfully.",
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

        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse>> SearchAssignations(string searchTerm = "", int position = 1, int pageSize = 10)
        {
            try
            {
                if (position < 1) position = 1;
                if (pageSize < 1) pageSize = 10;

                var assignations = await _assignationService.SearchAssignationsAsync(searchTerm, position, pageSize);
                int total = await _assignationService.CountAssignationsAsync(searchTerm);

                var viewBag = new Dictionary<string, object>
                {
                    { "nbrPerPage", pageSize },
                    { "TotalCount", total },
                    { "nbrLinks", (int)Math.Ceiling((double)total / pageSize) },
                    { "position", position }
                };

                return Ok(new ApiResponse
                {
                    Data = assignations,
                    ViewBag = viewBag,
                    IsSuccess = true,
                    Message = "Assignations retrieved successfully.",
                    StatusCode = 200
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Data = null,
                    ViewBag = null,
                    IsSuccess = false,
                    Message = $"Erreur lors de la recherche des assignations : {ex.Message}",
                    StatusCode = 500
                });
            }
        }

        [HttpGet("assigned-immobilisations")]
        public async Task<ActionResult<ApiResponse>> GetAssignedImmobilisations()
        {
            var assignedImmobilisations = await _assignationService.GetAssignedImmobilisationsAsync();
            return Ok(new ApiResponse
            {
                Data = assignedImmobilisations,
                ViewBag = null,
                IsSuccess = true,
                Message = "Assigned immobilisations retrieved successfully.",
                StatusCode = 200
            });
        }

        [HttpGet("current/{idImmobilisationPropre}")]
        public async Task<ActionResult<ApiResponse>> GetCurrentAssignation(int idImmobilisationPropre)
        {
            var assignation = await _assignationService.GetCurrentAssignationByImmobilisationIdAsync(idImmobilisationPropre);
            if (assignation == null)
            {
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Aucune assignation trouvée pour cette immobilisation.",
                    StatusCode = 404
                });
            }
            return Ok(new ApiResponse
            {
                Data = assignation,
                ViewBag = null,
                IsSuccess = true,
                Message = "Current assignation retrieved successfully.",
                StatusCode = 200
            });
        }
    }
}