namespace Moviemo.Models
{
    public class Comment
    {
        public long commentId {  get; set; }
        public string body { get; set; }
        public User author { get; set; }
        public long likeCounter { get; set; }
        public long dislikeCounter { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
