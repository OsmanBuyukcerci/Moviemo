using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Moviemo.Models;
using Moviemo.Services.Interfaces;

namespace Moviemo.Services
{
    public class TokenService(IConfiguration Configuration) : ITokenInterface
    {
        public string CreateToken(User User)
        {
            var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, User.Username),
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
