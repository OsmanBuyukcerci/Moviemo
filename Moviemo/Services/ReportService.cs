using Microsoft.EntityFrameworkCore;
using Moviemo.Data;
using Moviemo.Dtos;
using Moviemo.Dtos.Report;
using Moviemo.Models;
using Moviemo.Services.Interfaces;

namespace Moviemo.Services
{
    public class ReportService : IReportService
    {
        private readonly ILogger<ReportService> _Logger;

        private readonly AppDbContext _Context;

        public ReportService(AppDbContext Context, ILogger<ReportService> Logger)
        {
            _Logger = Logger;
            _Context = Context;
        }

        public async Task<List<ReportGetDto>?> GetAllAsync()
        {
            _Logger.LogInformation("Tüm rapor bilgileri alınıyor...");

            try
            {
                return await _Context.Reports
                .Include(R => R.User)
                .Select(R => new ReportGetDto
                {
                    Id = R.Id,
                    UserId = R.User.Id,
                    Title = R.Title,
                    Details = R.Details,
                    CreatedAt = R.CreatedAt
                })
                .ToListAsync();
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Tüm rapor bilgileri alınırken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<ReportGetDto?> GetByIdAsync(long Id)
        {
            _Logger.LogInformation("Report ID'si {Id} olan rapor bilgisi alınıyor...", Id);

            try
            {
                return await _Context.Reports
                .Include(R => R.User)
                .Select(R => new ReportGetDto
                {
                    Id = R.Id,
                    UserId = R.User.Id,
                    Title = R.Title,
                    Details = R.Details,
                    CreatedAt = R.CreatedAt
                })
                .FirstOrDefaultAsync(R => R.Id == Id);
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Rapor bilgisi alınırken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<ReportCreateDto?> CreateAsync(ReportCreateDto Dto, long UserId)
        {
            _Logger.LogInformation("Yeni rapor oluşturuluyor: {@ReportCreateDto}", Dto);

            try
            {
                var Report = new Report
                {
                    UserId = UserId,
                    Title = Dto.Title,
                    Details = Dto.Details
                };

                await _Context.Reports.AddAsync(Report);
                await _Context.SaveChangesAsync();

                _Logger.LogInformation("Rapor başarıyla oluşturuldu.");

                return Dto;
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Rapor oluşturulurken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<UpdateResponseDto?> UpdateAsync(long Id, long UserId, ReportUpdateDto Dto)
        {
            _Logger.LogInformation("Report ID'si {Id} olan rapor güncelleniyor: {@ReportUpdateDto}", Id , Dto);

            try
            {
                var Report = await _Context.Reports.FindAsync(Id);

                if (Report == null)
                    return new UpdateResponseDto { Issue = UpdateIssue.NotFound };

                if (Report.UserId !=  UserId)
                    return new UpdateResponseDto { Issue = UpdateIssue.NotOwner };

                var DtoProperties = Dto.GetType().GetProperties();
                var ReportType = Report.GetType();

                foreach (var Property in DtoProperties)
                {
                    var NewValue = Property.GetValue(Dto);
                    if (NewValue == null) continue;

                    var TargetProperty = ReportType.GetProperty(Property.Name);
                    if (TargetProperty == null || !TargetProperty.CanWrite) continue;

                    TargetProperty.SetValue(Report, NewValue);
                }

                await _Context.SaveChangesAsync();

                _Logger.LogInformation("Report ID'si {Id} olan rapor başarıyla güncellendi", Id);

                return new UpdateResponseDto { IsUpdated = true };
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Rapor güncellenirken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<DeleteResponseDto?> DeleteAsync(long Id, long UserId)
        {
            _Logger.LogInformation("Report ID'si {Id} olan rapor siliniyor...", Id);

            try
            {
                var Report = await _Context.Reports.FindAsync(Id);

                if (Report == null)
                    return new DeleteResponseDto { Issue = DeleteIssue.NotFound };

                if (Report.UserId != UserId)
                    return new DeleteResponseDto { Issue = DeleteIssue.NotOwner };

                _Context.Reports.Remove(Report);
                await _Context.SaveChangesAsync();

                _Logger.LogInformation("Report ID'si {Id} olan rapor başarıyla silindi.", Id);

                return new DeleteResponseDto { IsDeleted = true };
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Rapor silinirken bir hata meydana geldi.");
                return null;
            }
        }
    }
}
