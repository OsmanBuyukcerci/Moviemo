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
        [ForeignKey("UserId")]
        public required User User { get; set; }

        [Required]
        [ForeignKey("MovieId")]
        public required Movie Movie { get; set; }

        [Required]
        public required double UserScore { get; set; }

        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
