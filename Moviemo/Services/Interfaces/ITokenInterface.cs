using Moviemo.Dtos.Token;
using Moviemo.Models;

namespace Moviemo.Services.Interfaces
{
    public interface ITokenInterface
    {
        Task<TokenResponseDto> CreateTokenResponseAsync(User User);
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto Request);
        Task<User?> ValidateRefreshTokenAsync(long UserId, string RefreshToken);
        string GenerateRefreshToken();
        Task<string> GenerateAndSaveRefreshTokenAsync(User User);
        string CreateToken(User User);
    }
}
