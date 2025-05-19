using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Moviemo.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id {  get; set; }

        [Required]
        public required string Name {  get; set; }

        [Required]
        public required string Surname { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; } = String.Empty;

        [Required]
        public required string Email { get; set; }

        [Required]
        public required UserRole UserRole { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Report> Reports { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Vote> Votes { get; set; }
        public string? RefreshToken {  get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }

    public enum UserRole
    {
        Basic,
        Admin,
        Manager
    }
}
