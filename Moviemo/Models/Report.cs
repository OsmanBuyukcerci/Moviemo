namespace Moviemo.Models
{
    public class Report
    {
        public long reportId { get; set; }
        public string title { get; set; }
        public string details { get; set; }
        public User author { get; set; }
        public DateTime createdAt { get; set; }
    }
}
