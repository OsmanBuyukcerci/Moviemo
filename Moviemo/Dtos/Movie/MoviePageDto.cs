namespace Moviemo.Dtos.Movie
{
    public class MoviePageDto
    {
        public List<MovieGetDto> Data { get; set; } = new List<MovieGetDto>();
        public int Total { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
