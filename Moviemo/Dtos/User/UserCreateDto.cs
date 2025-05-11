using System.ComponentModel.DataAnnotations;
using Moviemo.Models;

namespace Moviemo.Dtos.User
{
    public class UserCreateDto
    {
        public required string Name { get; set; }

        public required string Surname { get; set; }

        public required string Username { get; set; }

        public required string Password { get; set; }

        public required string Email { get; set; }

        public required UserRole UserRole { get; set; }
    }
}
