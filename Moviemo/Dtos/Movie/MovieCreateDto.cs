using System.ComponentModel.DataAnnotations;

namespace Moviemo.Dtos.Movie
{
    public class MovieCreateDto
    {
        public required string Title { get; set; }

        public required string Overview { get; set; }

        public double TmdbScore { get; set; }

        public required string PosterPath { get; set; }

        public required string TrailerUrl { get; set; }
    }
}
