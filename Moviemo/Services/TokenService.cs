using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Moviemo.Data;
using Moviemo.Dtos.Token;
using Moviemo.Models;
using Moviemo.Services.Interfaces;

namespace Moviemo.Services
{
    public class TokenService(IConfiguration Configuration, AppDbContext Context, ILogger<TokenService> Logger) : ITokenService
    {
        public async Task<TokenResponseDto> CreateTokenResponseAsync(User User)
        {
            Logger.LogInformation("Token response oluşturuluyor...");

            try
            {
                var Response = new TokenResponseDto
                {
                    AccessToken = CreateToken(User),
                    RefreshToken = await GenerateAndSaveRefreshTokenAsync(User)
                };

                return Response;
            }
            catch (Exception Ex)
            {
                Logger.LogError(Ex, "Token response oluşturulurken bir hata meydana geldi.");
                return new TokenResponseDto {};
            }
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(long UserId, string RefreshToken)
        {
            Logger.LogInformation("Tokenler yenileniyor...");

            try
            {
                var User = await ValidateRefreshTokenAsync(UserId, RefreshToken);

                if (User is null) return null;

                Logger.LogInformation("Tokenler başarıyla yenilendi.");

                return await CreateTokenResponseAsync(User);
            }
            catch (Exception Ex)
            {
                Logger.LogError(Ex, "Tokenler yenilenirken bir hata meydana geldi.");
                return null;
            }
        }

        public async Task<User?> ValidateRefreshTokenAsync(long UserId, string RefreshToken)
        {
            Logger.LogInformation("Refresh token doğrulanıyor...");

            try
            {
                var User = await Context.Users.FindAsync(UserId);

                if (User == null ||
                    User.RefreshToken != RefreshToken ||
                    User.RefreshTokenExpiryTime <= DateTime.UtcNow)
                    return null;

                Logger.LogInformation("Refresh token başarıyla doğrulandı.");

                return User;
            }
            catch (Exception Ex)
            {
                Logger.LogError(Ex, "Refresh token doğrulanırken bir hata meydana geldi.");
                return null;
            }
        }

        public string GenerateRefreshToken()
        {
            Logger.LogInformation("Yeni refresh token oluşturuluyor...");

            try
            {
                var RandomNumber = new byte[32];
                using var Rng = RandomNumberGenerator.Create();
                Rng.GetBytes(RandomNumber);
                return Convert.ToBase64String(RandomNumber);
            }
            catch (Exception Ex)
            {
                Logger.LogError(Ex, "Refresh token oluşturulurken bir hata meydana geldi.");
                throw new ApplicationException("Refresh token oluşturulamadı, lütfen daha sonra tekrar deneyin.");
            }
        }

        public async Task<string> GenerateAndSaveRefreshTokenAsync(User User)
        {
            var RefreshToken = GenerateRefreshToken();
            User.RefreshToken = RefreshToken;
            User.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await Context.SaveChangesAsync();
            return RefreshToken;
        }

        public string CreateToken(User User)
        {
            Logger.LogInformation("Yeni JWT token oluşturuluyor...");

            try
            {
                var Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, User.Username),
                    new Claim(ClaimTypes.NameIdentifier, User.Id.ToString()),
                    new Claim(ClaimTypes.Role, User.UserRole.ToString())
                };

                var Key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Configuration.GetValue<string>("AppSettings:Token")!));

                var Creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512);

                var TokenDescriptor = new JwtSecurityToken(
                        issuer: Configuration.GetValue<string>("AppSettings:Issuer"),
                        audience: Configuration.GetValue<string>("AppSettings:Audience"),
                        claims: Claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: Creds
                );

                return new JwtSecurityTokenHandler().WriteToken(TokenDescriptor);
            }
            catch (Exception Ex)
            {
                Logger.LogError(Ex, "JWT token oluşturulurken bir hata meydana geldi.");
                throw new ApplicationException("JWT token oluşturulamadı, lütfen daha sonra tekrar deneyin.");
            }   
        }
    }
}
