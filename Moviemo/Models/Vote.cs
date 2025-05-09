using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviemo.Models
{
    public class Vote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VoteId {  get; set; }

        [Required]
        public required long UserId { get; set; }

        [Required]
        public required VoteType VoteType { get; set; }

        public DateTime VotedAt { get; set; } = DateTime.Now;

        [Required]
        public required long CommentId { get; set; }

        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }
    }

    public enum VoteType
    { 
        Downvote = -1,
        Upvote = 1
    }
}
