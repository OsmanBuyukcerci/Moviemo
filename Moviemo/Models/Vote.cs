using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviemo.Models
{
    public class Vote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id {  get; set; }

        [Required]
        public required long UserId { get; set; }

        public User User { get; set; }

        [Required]
        public required long CommentId { get; set; }

        public Comment Comment { get; set; }

        [Required]
        public required VoteType VoteType { get; set; }

        public DateTime VotedAt { get; set; } = DateTime.Now;
    }

    public enum VoteType
    { 
        Downvote = -1,
        Upvote = 1
    }
}
