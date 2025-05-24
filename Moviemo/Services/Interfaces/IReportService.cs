using Moviemo.Dtos;
using Moviemo.Dtos.Report;
using Moviemo.Models;

namespace Moviemo.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<ReportGetDto>?> GetAllAsync();
        Task<ReportGetDto?> GetByIdAsync(long Id);
        Task<ReportCreateDto?> CreateAsync(ReportCreateDto Dto, long UserId);
        Task<UpdateResponseDto?> UpdateAsync(long Id, long UserId, ReportUpdateDto Dto);
        Task<DeleteResponseDto?> DeleteAsync(long Id, long UserId);
    }
}
