namespace Moviemo.Dtos.Review
{
    public class ReviewGetDto
    {
        public long Id { get; set; }
        public required string Body { get; set; }
        public required long UserId { get; set; }
        public required long MovieId { get; set; }
        public double UserScore { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
    }
}
