using Moviemo.Dtos;
using Moviemo.Dtos.Token;
using Moviemo.Dtos.User;
using Moviemo.Models;

namespace Moviemo.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserGetDto>> GetAllAsync();
        Task<UserGetDto?> GetByIdAsync(long Id);
        Task<UserCreateDto?> CreateAsync(UserCreateDto Dto);
        Task<UpdateResponseDto> UpdateAsync(long Id, long UserId, UserUpdateDto Dto);
        Task<DeleteResponseDto> DeleteAsync(long Id, long UserId);
        Task<TokenResponseDto?> LoginAsync(UserLoginDto Dto);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto Dto);
    }
}
