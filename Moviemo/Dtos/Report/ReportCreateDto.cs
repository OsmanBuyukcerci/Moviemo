using System.ComponentModel.DataAnnotations;

namespace Moviemo.Dtos.Report
{
    public class ReportCreateDto
    {
        public required string Title { get; set; }

        public required string Details { get; set; }
    }
}
