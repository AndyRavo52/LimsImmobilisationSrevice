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
    [Route("api/report-immobilisations")]
    public class ReportImmobilisationsController : ControllerBase
    {
        private readonly IReportImmobilisationService _reportService;

        public ReportImmobilisationsController(IReportImmobilisationService reportService)
        {
            _reportService = reportService;
        }

        // Récupère le nombre total de rapports
        [HttpGet("total")]
        public async Task<ActionResult<ApiResponse>> GetTotalReports()
        {
            int total = await _reportService.CountReportImmobilisationsAsync();
            return Ok(new ApiResponse
            {
                Data = total,
                ViewBag = null,
                IsSuccess = true,
                Message = "Total des rapports d'immobilisation récupéré avec succès.",
                StatusCode = 200
            });
        }

        // Récupère une liste paginée de rapports
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetReports(int position = 1, int pageSize = 10)
        {
            if (position < 1) position = 1;
            if (pageSize < 1) pageSize = 10;

            var reports = await _reportService.GetReportImmobilisationsAsync(position, pageSize);
            int total = await _reportService.CountReportImmobilisationsAsync();

            var viewBag = new Dictionary<string, object>
            {
                { "nbrPerPage", pageSize },
                { "TotalCount", total },
                { "nbrLinks", (int)Math.Ceiling((double)total / pageSize) },
                { "position", position }
            };

            return Ok(new ApiResponse
            {
                Data = reports,
                ViewBag = viewBag,
                IsSuccess = true,
                Message = "Rapports d'immobilisation récupérés avec succès.",
                StatusCode = 200
            });
        }

        // Récupère un rapport par son ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetReportById(int id)
        {
            try
            {
                var report = await _reportService.GetReportImmobilisationByIdAsync(id);
                return Ok(new ApiResponse
                {
                    Data = report,
                    ViewBag = null,
                    IsSuccess = true,
                    Message = "Rapport d'immobilisation récupéré avec succès.",
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

        // Crée un nouveau rapport
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateReport([FromBody] ReportImmobilisationDto dto)
        {
            var created = await _reportService.CreateReportImmobilisationAsync(dto);
            return CreatedAtAction(nameof(GetReportById), new { id = created.IdReportImmobilisation }, new ApiResponse
            {
                Data = created,
                ViewBag = null,
                IsSuccess = true,
                Message = "Rapport d'immobilisation créé avec succès.",
                StatusCode = 201
            });
        }
    }
}
