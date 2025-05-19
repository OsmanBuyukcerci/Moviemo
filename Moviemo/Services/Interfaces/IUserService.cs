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
        Task<bool> UpdateAsync(long Id, UserUpdateDto Dto);
        Task<bool> DeleteAsync(long Id);
        Task<TokenResponseDto?> LoginAsync(UserLoginDto Dto);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto Dto);
    }
}
