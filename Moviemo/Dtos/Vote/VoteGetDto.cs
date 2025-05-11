using Moviemo.Models;

namespace Moviemo.Dtos.Vote
{
    public class VoteGetDto
    {
        public long Id { get; set; }
        public required long UserId { get; set; }
        public required long CommentId { get; set; }
        public required VoteType VoteType {  get; set; }
    }
}
