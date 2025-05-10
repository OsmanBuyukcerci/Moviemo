namespace Moviemo.Dtos.Comment
{
    public class CommentGetDto
    {
        public long Id { get; set; }
        public required string Body { get; set; }
        public required long UserId { get; set; } = -1;
        public required long MovieId { get; set; } = -1;
        public DateTime CreatedAt { get; set; }
    }
}
