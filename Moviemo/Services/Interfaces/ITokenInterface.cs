using Moviemo.Models;

namespace Moviemo.Services.Interfaces
{
    public interface ITokenInterface
    {
        string CreateToken(User User);
    }
}
