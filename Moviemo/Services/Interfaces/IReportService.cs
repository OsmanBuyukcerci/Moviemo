using Moviemo.Dtos.Report;
using Moviemo.Models;

namespace Moviemo.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<ReportGetDto>> GetAllAsync();
        Task<ReportGetDto?> GetByIdAsync(long Id);
        Task<ReportCreateDto> CreateAsync(ReportCreateDto Dto);
        Task<bool> UpdateAsync(long Id, ReportUpdateDto Dto);
        Task<bool> DeleteAsync(long Id);
    }
}
