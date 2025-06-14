using Moviemo.Models;

namespace Moviemo.Dtos.User
{
    public class UserUpdateDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public UserRole? UserRole { get; set; }
    }
}
