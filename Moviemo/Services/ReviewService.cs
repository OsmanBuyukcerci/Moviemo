using Microsoft.EntityFrameworkCore;
using Moviemo.Data;
using Moviemo.Dtos;
using Moviemo.Dtos.Review;
using Moviemo.Models;
using Moviemo.Services.Interfaces;

namespace Moviemo.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ILogger<ReviewService> _Logger;

        private readonly AppDbContext _Context;

        public ReviewService(AppDbContext Context, ILogger<ReviewService> Logger)
        {
            _Logger = Logger;
            _Context = Context;
        }
        public async Task<List<ReviewGetDto>?> GetAllAsync()
        {
            _Logger.LogInformation("Tüm inceleme bilgileri alınıyor...");

            try
            {
                return await _Context.Reviews
                .Include(R => R.User)
                .Include(R => R.Movie)
                .Select(R => new ReviewGetDto
                {
                    Id = R.Id,
                    Body = R.Body,
                    UserId = R.User.Id,
                    MovieId = R.Movie.Id,
                    UserScore = R.UserScore,
                    CreatedAt = R.CreatedAt,
                    UpdatedAt = R.UpdatedAt,
                })
                .ToListAsync();
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Tüm inceleme bilgileri alınırken bir hata meydana geldi."); ;
                return null;
            }
        }

        public async Task<ReviewGetDto?> GetByIdAsync(long Id)
        {
            _Logger.LogInformation("Review ID'si {Id} olan inceleme bilgisi alınıyor.", Id);

            try
            {
                return await _Context.Reviews
                .Include(R => R.User)
                .Include(R => R.Movie)
                .Select(R => new ReviewGetDto
                {
                    Id = R.Id,
                    Body = R.Body,
                    UserId = R.User.Id,
                    MovieId = R.Movie.Id,
                    UserScore = R.UserScore,
                    CreatedAt = R.CreatedAt,
                    UpdatedAt = R.UpdatedAt
                }).FirstOrDefaultAsync(R => R.Id == Id);
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "İnceleme bilgisi alınırken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<List<ReviewGetDto>?> GetByMovieIdAsync(long? MovieId)
        {
            _Logger.LogInformation("Filme ait tüm incelemeler alınıyor...");

            try
            {
                return await _Context.Reviews.Where(R => R.MovieId == MovieId).Select(R => new ReviewGetDto
                {
                    Id = R.Id,
                    Body = R.Body,
                    UserId = R.User.Id,
                    MovieId = R.Movie.Id,
                    UserScore = R.UserScore,
                    CreatedAt = R.CreatedAt,
                    UpdatedAt = R.UpdatedAt
                }).ToListAsync();
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "Filme ait incelemeler alınırken bir sorun meydana geldi.");
                return null;
            }
        }

        public async Task<ReviewCreateDto?> CreateAsync(ReviewCreateDto Dto, long UserId)
        {
            _Logger.LogInformation("Yeni inceleme oluşturuluyor: {@ReviewCreateDto}", Dto);

            try
            {
                var Review = new Review
                {
                    Body = Dto.Body,
                    UserId = UserId,
                    MovieId = Dto.MovieId,
                    UserScore = Dto.UserScore
                };

                await _Context.Reviews.AddAsync(Review);
                await _Context.SaveChangesAsync();

                _Logger.LogInformation("İnceleme başarıyla oluşturuldu.");

                return Dto;
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "İnceleme oluşturulurken bir hata meydana geldi.");
                return null;
            }

            
        }

        public async Task<UpdateResponseDto?> UpdateAsync(long Id, long UserId, ReviewUpdateDto Dto)
        {
            _Logger.LogInformation("Review ID'si {Id} olan inceleme güncelleniyor: {@ReviewUpdateDto}", Id, Dto);

            try
            {
                var Review = await _Context.Reviews.FindAsync(Id);

                if (Review == null)
                    return new UpdateResponseDto { Issue = UpdateIssue.NotFound };

                if (Review.UserId !=  UserId)
                    return new UpdateResponseDto { Issue = UpdateIssue.NotOwner };

                var DtoProperties = Dto.GetType().GetProperties();
                var ReviewType = Review.GetType();

                foreach (var Property in DtoProperties)
                {
                    var NewValue = Property.GetValue(Dto);
                    if (NewValue == null) continue;

                    var TargetProperty = ReviewType.GetProperty(Property.Name);
                    if (TargetProperty == null || !TargetProperty.CanWrite) continue;

                    TargetProperty.SetValue(Review, NewValue);
                }

                Review.UpdatedAt = DateTime.UtcNow;

                await _Context.SaveChangesAsync();

                _Logger.LogInformation("Review ID'si {Id} olan inceleme başarıyla güncellendi.", Id);

                return new UpdateResponseDto { IsUpdated = true };
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "İnceleme güncellenirken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<DeleteResponseDto?> DeleteAsync(long Id, long UserId)
        {
            _Logger.LogInformation("Review ID'si {Id} olan inceleme siliniyor...", Id);

            try
            {
                var Review = await _Context.Reviews.FindAsync(Id);

                if (Review == null)
                    return new DeleteResponseDto { Issue = DeleteIssue.NotFound };

                if (Review.UserId != UserId)
                    return new DeleteResponseDto { Issue = DeleteIssue.NotOwner };

                _Context.Reviews.Remove(Review);
                await _Context.SaveChangesAsync();

                _Logger.LogInformation("Review ID'si {Id} olan inceleme başarıyla silindi.", Id);

                return new DeleteResponseDto { IsDeleted = true };
            }
            catch (Exception Ex)
            {
                _Logger.LogError(Ex, "İnceleme silinirken bir hata meydana geldi");
                return null;
            }
        }
    }
}