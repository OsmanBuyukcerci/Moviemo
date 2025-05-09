using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Moviemo.Models
{
    public class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReportId { get; set; }

        [Required]
        public required long UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Details { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
