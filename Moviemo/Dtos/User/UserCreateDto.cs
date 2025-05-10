using System.ComponentModel.DataAnnotations;
using Moviemo.Models;

namespace Moviemo.Dtos.User
{
    public class UserCreateDto
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Surname { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public required UserRole UserRole { get; set; }
    }
}
