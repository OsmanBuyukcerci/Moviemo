using System.ComponentModel.DataAnnotations;

namespace Moviemo.Dtos.Comment
{
    public class CommentCreateDto
    {
        public required string Body { get; set; }

        public required long MovieId { get; set; }
    }
}
