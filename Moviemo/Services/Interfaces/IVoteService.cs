using Moviemo.Dtos.Vote;
using Moviemo.Models;

namespace Moviemo.Services.Interfaces
{
    public interface IVoteService
    {
        Task<List<VoteGetDto>> GetAllAsync();
        Task<VoteGetDto?> GetByIdAsync(long Id);
        Task<VoteCreateDto> CreateAsync(VoteCreateDto Dto);
        Task<bool> UpdateAsync(long Id, VoteUpdateDto Dto);
        Task<bool> DeleteAsync(long Id);
    }
}
