using Moviemo.Dtos;
using Moviemo.Dtos.Vote;

namespace Moviemo.Services.Interfaces
{
    public interface IVoteService
    {
        Task<List<VoteGetDto>?> GetAllAsync();
        Task<VoteGetDto?> GetByIdAsync(long Id);
        Task<VoteGetDto?> GetByUserAndCommentIdAsync(long? UserId, long? CommentId);
        Task<CreateResponseDto?> CreateAsync(VoteCreateDto Dto, long UserId);
        Task<UpdateResponseDto?> UpdateAsync(long Id, long UserId, VoteUpdateDto Dto);
        Task<DeleteResponseDto?> DeleteAsync(long Id, long UserId);
    }
}
