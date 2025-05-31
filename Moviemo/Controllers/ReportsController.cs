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

            if (Reports == null)
                return StatusCode(500, "Tüm rapor bilgileri alınırken bir sunucu hatası meydana geldi.");

            return Ok(Reports);
        }

        // api/reports/{Id} -> Rotada belirtilen ID'ye sahip rapor bilgisini al
        [Authorize(Roles = "Manager")]
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetReportById(long Id)
        {
            var Report = await _ReportService.GetByIdAsync(Id);

            if (Report == null) 
                return NotFound();

            return Ok(Report);
        }

        // api/reports -> Rapor oluştur
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] ReportCreateDto Dto)
        {
            if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var UserId))
                return Unauthorized("Geçersiz kullanıcı token bilgisi.");

            var Report = await _ReportService.CreateAsync(Dto, UserId);

            if (Report == null)
                return StatusCode(500, "Rapor oluşturulurken bir sunucu hatası meydana geldi.");

            return Ok(Dto);
        }

        // api/reports/{Id} -> Rotada belirtilen ID'ye sahip raporu güncelle
        [Authorize(Roles = "Manager")]
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateReport(long Id, ReportUpdateDto Dto)
        {
            if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var UserId))
                return Unauthorized("Geçersiz kullanıcı token bilgisi.");

            var ResponseDto = await _ReportService.UpdateAsync(Id, UserId, Dto);

            if (ResponseDto == null)
                return StatusCode(500, "Rapor güncellenirken bir sunucu hatası meydana geldi.");

            if (ResponseDto.IsUpdated) return Ok(Dto);

            return ResponseDto.Issue switch
            { 
                UpdateIssue.NotFound => NotFound($"Report ID'si {Id} olan rapor bulunamadı."),
                UpdateIssue.NotOwner => Unauthorized("Size ait olmayan bir incelemeyi güncelleyemezsiniz."),
                _ => BadRequest("Rapor güncelleme işlemi gerçekleştirilemedi.")
            };
        }

        // api/reports/{Id} -> Rotada belirtilen ID'ye sahip raporu sil}
        [Authorize(Roles = "Manager")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteReport(long Id)
        {
            if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var UserId))
                return Unauthorized("Geçersiz kullanıcı token bilgisi.");

            var ResponseDto = await _ReportService.DeleteAsync(Id, UserId);

            if (ResponseDto == null)
                return StatusCode(500, "Rapor silinirken bir sunucu hatası meydana geldi.");

            if (ResponseDto.IsDeleted) return NoContent();

            return ResponseDto.Issue switch
            {
                DeleteIssue.NotFound => NotFound($"Report ID'si {Id} olan rapor bulunamadı."),
                DeleteIssue.NotOwner => Unauthorized("Size ait olmayan bir incelemeyi silemezsiniz."),
                _ => BadRequest("Rapor silme işlemi gerçekleştirilemedi.")
            };
        }
    }
}
