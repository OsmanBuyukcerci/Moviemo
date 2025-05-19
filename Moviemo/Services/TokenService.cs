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
    public class TokenService(IConfiguration Configuration, AppDbContext Context) : ITokenInterface
    {
        public async Task<TokenResponseDto> CreateTokenResponseAsync(User User)
        {
            var Response = new TokenResponseDto
            {
                AccessToken = CreateToken(User),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(User)
            };

            return Response;
        }
        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto Request)
        {
            var User = await ValidateRefreshTokenAsync(Request.UserId, Request.RefreshToken);

            if (User is null) return null;

            return await CreateTokenResponseAsync(User);
        }

        public async Task<User?> ValidateRefreshTokenAsync(long UserId, string RefreshToken)
        {
            var User = await Context.Users.FindAsync(UserId);

            if (User == null || 
                User.RefreshToken != RefreshToken || 
                User.RefreshTokenExpiryTime <= DateTime.UtcNow) 
                return null;

            return User;
        }

        public string GenerateRefreshToken()
        {
            var RandomNumber = new byte[32];
            using var Rng = RandomNumberGenerator.Create();
            Rng.GetBytes(RandomNumber);
            return Convert.ToBase64String(RandomNumber);
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
    }
}
