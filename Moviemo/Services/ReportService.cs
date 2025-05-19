using System.Reflection.Metadata.Ecma335;
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
        private readonly AppDbContext _Context;

        public ReportService(AppDbContext Context)
        {
            _Context = Context;
        }

        public async Task<List<ReportGetDto>> GetAllAsync()
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

        public async Task<ReportGetDto?> GetByIdAsync(long Id)
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

        public async Task<ReportCreateDto> CreateAsync(ReportCreateDto Dto)
        {
            var Report = new Report
            {
                UserId = Dto.UserId,
                Title = Dto.Title,
                Details = Dto.Details
            };

            await _Context.Reports.AddAsync(Report);
            await _Context.SaveChangesAsync();

            return Dto;
        }

        public async Task<UpdateResponseDto> UpdateAsync(long Id, long UserId, ReportUpdateDto Dto)
        {
            var ResponseDto = new UpdateResponseDto
            {
                IsUpdated = false,
                Issue = UpdateIssue.None
            };

            var Report = await _Context.Reports.FindAsync(Id);

            if (Report == null)
            {
                ResponseDto.Issue = UpdateIssue.NotFound;
                return ResponseDto;
            }

            else if (Report.UserId != UserId)
            {
                ResponseDto.Issue = UpdateIssue.Unauthorized;
                return ResponseDto;
            }

            var DtoProperties = Dto.GetType().GetProperties();
            var ReportType = Report.GetType();

            foreach(var Property in DtoProperties)
            {
                var NewValue = Property.GetValue(Dto);
                if (NewValue == null) continue;

                var TargetProperty = ReportType.GetProperty(Property.Name);
                if (TargetProperty == null || !TargetProperty.CanWrite) continue;

                TargetProperty.SetValue(Report, NewValue);
            }

            await _Context.SaveChangesAsync();

            ResponseDto.IsUpdated = true;

            return ResponseDto;
        }

        public async Task<DeleteResponseDto> DeleteAsync(long Id, long UserId)
        {
            var ResponseDto = new DeleteResponseDto
            {
                IsDeleted = false,
                Issue = DeleteIssue.None
            };

            var Report = await _Context.Reports.FindAsync(Id);

            if (Report == null)
            {
                ResponseDto.Issue = DeleteIssue.NotFound;
                return ResponseDto;
            } 
            
            else if (Report.UserId != UserId)
            {
                ResponseDto.Issue = DeleteIssue.Unauthorized;
                return ResponseDto;
            }

            _Context.Reports.Remove(Report);
            await _Context.SaveChangesAsync();

            ResponseDto.IsDeleted = true;

            return ResponseDto;
        }
    }
}
