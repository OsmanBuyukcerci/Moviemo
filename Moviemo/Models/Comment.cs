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
        public required long UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public required long MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
