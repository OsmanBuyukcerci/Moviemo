using System.ComponentModel.DataAnnotations;

namespace Moviemo.Dtos.Comment
{
    public class CommentCreateDto
    {
        [Required]
        public required string Body { get; set; }

        [Required]
        public required long UserId { get; set; }

        [Required]
        public required long MovieId { get; set; }
    }
}
