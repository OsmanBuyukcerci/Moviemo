using System.ComponentModel.DataAnnotations;

namespace Moviemo.Dtos.Review
{
    public class ReviewCreateDto
    {
        public required string Body { get; set; }

        public required long MovieId { get; set; }

        public required double UserScore { get; set; }
    }
}
