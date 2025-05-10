using System.ComponentModel.DataAnnotations;

namespace Moviemo.Dtos.Movie
{
    public class MovieCreateDto
    {
        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Overview { get; set; }

        public double TmdbScore { get; set; }

        [Required]
        public required string PosterPath { get; set; }

        [Required]
        public required string TrailerUrl { get; set; }
    }
}
