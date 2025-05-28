namespace Moviemo.Dtos.Movie
{
    public class SearchResponseDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string PosterPath {  get; set; } = string.Empty;
    }
}
