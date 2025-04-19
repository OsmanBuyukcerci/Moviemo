using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Moviemo.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long movieId { get; set; }
        public string title { get; set; }
        public string overview { get; set; }
        public double tmdbScore { get; set; }
        public string posterPath { get; set; }
        public string trailerUrl { get; set; }
        ICollection<Review> reviews { get; set; }
        ICollection<Comment> comments { get; set; }
    }
}
