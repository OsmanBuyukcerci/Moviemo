using Moviemo.Dtos;
using Moviemo.Dtos.Review;

namespace Moviemo.Services.Interfaces
{
    public interface IReviewService
    {
        Task<List<ReviewGetDto>?> GetAllAsync();
        Task<ReviewGetDto?> GetByIdAsync(long Id);
        Task<List<ReviewGetDto>?> GetByMovieIdAsync(long? MovieId);
        Task<ReviewCreateDto?> CreateAsync(ReviewCreateDto Dto, long UserId);
        Task<UpdateResponseDto?> UpdateAsync(long Id, long UserId, ReviewUpdateDto Dto);
        Task<DeleteResponseDto?> DeleteAsync(long Id, long UserId);
    }
}
