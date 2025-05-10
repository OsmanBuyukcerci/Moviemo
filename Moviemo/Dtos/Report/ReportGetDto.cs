namespace Moviemo.Dtos.Report
{
    public class ReportGetDto
    {
        public long Id { get; set; }
        public required long UserId { get; set; }
        public required string Title { get; set; }
        public required string Details { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
