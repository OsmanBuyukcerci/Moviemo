using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Moviemo.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MovieId { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Overview { get; set; }

        public double TmdbScore { get; set; }

        [Required]
        public required string PosterPath { get; set; }

        [Required]
        public required string TrailerUrl { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
