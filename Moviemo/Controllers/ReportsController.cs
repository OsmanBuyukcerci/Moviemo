using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            var Reports = await _ReportService.GetAllAsync();

            return Ok(Reports);
        }

        // api/reports/{Id} -> Rotada belirtilen ID'ye sahip rapor bilgisini al
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetReportById(long Id)
        {
            var Report = await _ReportService.GetByIdAsync(Id);

            if (Report == null) return NotFound();

            return Ok(Report);
        }

        // api/reports -> Rapor oluştur
        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] ReportCreateDto Dto)
        {
            var Report = await _ReportService.CreateAsync(Dto);

            return Ok(Dto);
        }

        // api/reports/{Id} -> Rotada belirtilen ID'ye sahip raporu güncelle
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateReport(long Id, ReportUpdateDto Dto)
        {
            bool IsUpdated = await _ReportService.UpdateAsync(Id, Dto);

            if (!IsUpdated) return BadRequest();

            return Ok(Dto);
        }

        // api/reports/{Id} -> Rotada belirtilen ID'ye sahip raporu sil}
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteReport(long Id)
        {
            bool IsDeleted = await _ReportService.DeleteAsync(Id);

            if (!IsDeleted) return NotFound();

            return NoContent();
        }
    }
}
