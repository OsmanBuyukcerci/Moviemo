using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviemo.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CommentId {  get; set; }

        [Required]
        public required string Body { get; set; }

        [Required]
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public required User User { get; set; }

        [Required]
        public required long MovieId { get; set; }

        [ForeignKey("MovieId")]
        public required Movie Movie { get; set; }

        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
