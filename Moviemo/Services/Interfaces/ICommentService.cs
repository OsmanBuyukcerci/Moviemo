using Moviemo.Dtos.Comment;

namespace Moviemo.Services.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentGetDto>> GetAllAsync();
        Task<CommentGetDto?> GetByIdAsync(long Id);
        Task<CommentCreateDto> CreateAsync(CommentCreateDto Dto);
        Task<bool> UpdateAsync(long Id, CommentUpdateDto Dto);
        Task<bool> DeleteAsync(long Id);
    }
}
