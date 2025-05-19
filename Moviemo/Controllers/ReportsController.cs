using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moviemo.Dtos;
using Moviemo.Dtos.Report;
using Moviemo.Services.Interfaces;

namespace Moviemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _ReportService;

        public ReportsController(IReportService ReportService)
        {
            _ReportService = ReportService;
        }

        // api/reports -> Tüm rapor bilgilerini al
        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            var Reports = await _ReportService.GetAllAsync();

            return Ok(Reports);
        }

        // api/reports/{Id} -> Rotada belirtilen ID'ye sahip rapor bilgisini al
        [Authorize(Roles = "Manager")]
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetReportById(long Id)
        {
            var Report = await _ReportService.GetByIdAsync(Id);

            if (Report == null) return NotFound();

            return Ok(Report);
        }

        // api/reports -> Rapor oluştur
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] ReportCreateDto Dto)
        {
            var Report = await _ReportService.CreateAsync(Dto);

            return Ok(Dto);
        }

        // api/reports/{Id} -> Rotada belirtilen ID'ye sahip raporu güncelle
        [Authorize]
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateReport(long Id, ReportUpdateDto Dto)
        {
            var UserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var ResponseDto = await _ReportService.UpdateAsync(Id, UserId, Dto);

            if (ResponseDto.IsUpdated) return Ok(Dto);

            else if (ResponseDto.Issue == UpdateIssue.NotFound)
                return NotFound($"Report ID'si {Id} olan rapor bulunamadı.");

            else if (ResponseDto.Issue == UpdateIssue.Unauthorized)
                return Unauthorized("Size ait olmayan bir raporu güncelleyemezsiniz.");

            return BadRequest("Rapor güncelleme işlemi gerçekleştirilemedi.");
        }

        // api/reports/{Id} -> Rotada belirtilen ID'ye sahip raporu sil}
        [Authorize]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteReport(long Id)
        {
            var UserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var ResponseDto = await _ReportService.DeleteAsync(Id, UserId);

            if (ResponseDto.IsDeleted) return NoContent();

            else if (ResponseDto.Issue == DeleteIssue.NotFound)
                return NotFound($"Report ID'si {Id} olan rapor bulunamadı.");

            else if (ResponseDto.Issue == DeleteIssue.Unauthorized)
                return Unauthorized("Size ait olmayan bir raporu silemezsiniz.");

            return BadRequest("Rapor silme işlemi gerçekleştirilemedi.");
        }
    }
}
