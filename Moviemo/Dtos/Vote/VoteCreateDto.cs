using System.ComponentModel.DataAnnotations;
using Moviemo.Models;

namespace Moviemo.Dtos.Vote
{
    public class VoteCreateDto
    {
        [Required]
        public required long UserId { get; set; }

        [Required]
        public required VoteType VoteType { get; set; }

        [Required]
        public required long CommentId { get; set; }
    }
}
