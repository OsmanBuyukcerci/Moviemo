using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Moviemo.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReviewId { get; set; }

        [Required]
        public required string Body { get; set; }

        [Required]
        public required long UserId { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        [Required]
        public required long MovieId { get; set; }

        [ForeignKey("MovieId")]
        public required Movie Movie { get; set; }

        [Required]
        public required double UserScore { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
