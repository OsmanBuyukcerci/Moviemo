using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Moviemo.Data;
using Moviemo.Dtos.Review;
using Moviemo.Models;
using Moviemo.Services.Interfaces;

namespace Moviemo.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _Context;

        public ReviewService(AppDbContext Context)
        {
            _Context = Context;
        }
        public async Task<List<ReviewGetDto>> GetAllAsync()
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

        public async Task<ReviewGetDto?> GetByIdAsync(long Id)
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

        public async Task<ReviewCreateDto> CreateAsync(ReviewCreateDto Dto)
        {
            var Review = new Review 
            {
                Body = Dto.Body,
                UserId = Dto.UserId,
                MovieId = Dto.MovieId,
                UserScore = Dto.UserScore
            };

            await _Context.Reviews.AddAsync(Review); 
            await _Context.SaveChangesAsync();

            return Dto;
        }

        public async Task<bool> UpdateAsync(long Id, ReviewUpdateDto Dto)
        {
            var Review = await _Context.Reviews.FindAsync(Id);

            if (Review == null) return false;

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

            return true;
        }

        public async Task<bool> DeleteAsync(long Id)
        {
            var Review = await _Context.Reviews.FindAsync(Id);

            if (Review == null) return false;

            _Context.Reviews.Remove(Review);
            await _Context.SaveChangesAsync();

            return true;
        }
    }
}