namespace Moviemo.Models
{
    public class Review
    {
        public long reviewId { get; set; }
        public string body { get; set; }
        public User author { get; set; }
        public Movie reviewedMovie { get; set; }
        public double userScore { get; set; }
        public long likeCounter { get; set; }
        public long dislikeCounter { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
