using System.ComponentModel.DataAnnotations;
using Moviemo.Models;

namespace Moviemo.Dtos.Vote
{
    public class VoteCreateDto
    {
        public required VoteType VoteType { get; set; }

        public required long CommentId { get; set; }
    }
}
