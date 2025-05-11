namespace Moviemo.Dtos.Comment
{
    public class CommentGetDto
    {
        public long Id { get; set; }
        public required string Body { get; set; }
        public required long UserId { get; set; } = -1;
        public required long MovieId { get; set; } = -1;
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
        public required int DownvoteCounter { get; set; }
        public required int UpvoteCounter { get; set; }
    }
}
