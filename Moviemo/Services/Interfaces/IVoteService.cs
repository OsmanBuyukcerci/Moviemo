using Moviemo.Dtos;
using Moviemo.Dtos.Vote;
using Moviemo.Models;

namespace Moviemo.Services.Interfaces
{
    public interface IVoteService
    {
        Task<List<VoteGetDto>?> GetAllAsync();
        Task<VoteGetDto?> GetByIdAsync(long Id);
        Task<CreateResponseDto?> CreateAsync(VoteCreateDto Dto, long UserId);
        Task<UpdateResponseDto?> UpdateAsync(long Id, long UserId, VoteUpdateDto Dto);
        Task<DeleteResponseDto?> DeleteAsync(long Id, long UserId);
    }
}
