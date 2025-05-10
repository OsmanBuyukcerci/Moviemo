using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Moviemo.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public required string Body { get; set; }

        [Required]
        public required long UserId { get; set; }

        public User User { get; set; }

        [Required]
        public required long MovieId { get; set; }

        public Movie Movie { get; set; }

        [Required]
        public required double UserScore { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
