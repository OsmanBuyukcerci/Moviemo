using System.ComponentModel.DataAnnotations;

namespace Moviemo.Dtos.Report
{
    public class ReportCreateDto
    {
        [Required]
        public required long UserId { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Details { get; set; }
    }
}
