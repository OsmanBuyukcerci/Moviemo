namespace Moviemo.Dtos.Vote
{
    public class VoteGetDto
    {
        public long Id { get; set; }
        public required long UserId { get; set; }
        public long CommentId { get; set; }
    }
}
