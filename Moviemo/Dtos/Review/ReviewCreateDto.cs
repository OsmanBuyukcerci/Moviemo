using System.ComponentModel.DataAnnotations;

namespace Moviemo.Dtos.Review
{
    public class ReviewCreateDto
    {
        [Required]
        public required string Body { get; set; }

        [Required]
        public required long UserId { get; set; }

        [Required]
        public required long MovieId { get; set; }

        [Required]
        public required double UserScore { get; set; }
    }
}
