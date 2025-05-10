using Moviemo.Dtos.Comment;
using Moviemo.Dtos.Review;
using Moviemo.Models;

namespace Moviemo.Dtos.Movie
{
    public class MovieGetDto
    {
        public long Id { get; set; }
        public required string Title { get; set; }
        public required string Overview { get; set; }
        public double TmdbScore { get; set; }
        public required string PosterPath { get; set; }
        public required string TrailerUrl { get; set; }
        public required ICollection<ReviewGetDto> Reviews { get; set; }
        public required ICollection<CommentGetDto> Comments { get; set; }
    }
}
