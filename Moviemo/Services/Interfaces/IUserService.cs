using Moviemo.Dtos;
using Moviemo.Dtos.Token;
using Moviemo.Dtos.User;

namespace Moviemo.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserGetDto>?> GetAllAsync();
        Task<UserGetDto?> GetByIdAsync(long Id);

        Task<UserGetDto?> GetByUsernameAsync(string Username);
        Task<CreateResponseDto?> CreateAsync(UserCreateDto Dto);
        Task<UpdateResponseDto?> UpdateAsync(long Id, long UserId, UserUpdateDto Dto);
        Task<DeleteResponseDto?> DeleteAsync(long Id, long UserId);
        Task<LoginResponseDto?> LoginAsync(UserLoginDto Dto);
        Task<TokenResponseDto?> RefreshTokensAsync(long UserId, RefreshTokenRequestDto Dto);
        Task<PasswordChangeResponseDto?> ChangePasswordAsync (ChangePasswordDto Dto, long Id, long UserId);
    }
}
