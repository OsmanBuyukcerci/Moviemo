using Moviemo.Dtos.Comment;
using Moviemo.Dtos.Review;
using Moviemo.Models;

namespace Moviemo.Dtos.User
{
    public class UserGetDto
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public UserRole UserRole { get; set; }
        public ICollection<ReviewGetDto> Reviews { get; set; } = new List<ReviewGetDto>();
        public ICollection<CommentGetDto> Comments { get; set; } = new List<CommentGetDto>();
    }
}
