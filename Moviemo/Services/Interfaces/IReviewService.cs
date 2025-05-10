using Moviemo.Dtos.Review;

namespace Moviemo.Services.Interfaces
{
    public interface IReviewService
    {
        Task<List<ReviewGetDto>> GetAllAsync();
        Task<ReviewGetDto?> GetByIdAsync(long Id);
        Task<ReviewCreateDto> CreateAsync(ReviewCreateDto Dto);
        Task<bool> UpdateAsync(long Id, ReviewUpdateDto Dto);
        Task<bool> DeleteAsync(long Id);
    }
}
