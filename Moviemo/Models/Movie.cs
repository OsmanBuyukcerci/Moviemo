using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Moviemo.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Overview { get; set; }

        [Required]
        public required string PosterPath { get; set; }

        [Required]
        public required string TrailerUrl { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
