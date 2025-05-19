using Moviemo.Dtos;
using Moviemo.Dtos.Comment;

namespace Moviemo.Services.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentGetDto>> GetAllAsync();
        Task<CommentGetDto?> GetByIdAsync(long Id);
        Task<CommentCreateDto> CreateAsync(CommentCreateDto Dto);
        Task<UpdateResponseDto> UpdateAsync(long Id, long UserId, CommentUpdateDto Dto);
        Task<DeleteResponseDto> DeleteAsync(long Id, long UserId);
    }
}
