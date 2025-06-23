using Moviemo.Dtos.Token;
using Moviemo.Models;

namespace Moviemo.Services.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponseDto> CreateTokenResponseAsync(User User);
        Task<TokenResponseDto?> RefreshTokensAsync(long UserId, string RefreshToken);
        Task<User?> ValidateRefreshTokenAsync(long UserId, string RefreshToken);
        string GenerateRefreshToken();
        Task<string> GenerateAndSaveRefreshTokenAsync(User User);
        string CreateToken(User User);
    }
}
