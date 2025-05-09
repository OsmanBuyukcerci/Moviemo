using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Moviemo.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserId {  get; set; }

        [Required]
        public required string Name {  get; set; }

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

    public enum UserRole
    {
        Basic,
        Admin,
        Manager
    }
}
