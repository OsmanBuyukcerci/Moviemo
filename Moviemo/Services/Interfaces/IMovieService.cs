using Moviemo.Dtos;
using Moviemo.Dtos.Movie;
using Moviemo.Models;

namespace Moviemo.Services.Interfaces
{
    public interface IMovieService
    {
        Task<List<MovieGetDto>?> GetAllAsync();
        Task<MovieGetDto?> GetByIdAsync(long Id);
        Task<MovieCreateDto?> CreateAsync(MovieCreateDto Dto);
        Task<UpdateResponseDto?> UpdateAsync(long Id, MovieUpdateDto Dto);
        Task<DeleteResponseDto?> DeleteAsync(long Id);
        Task<List<SearchResponseDto>?> SearchAsync(string Query);
        Task<MoviePageDto?> GetByPageSizeAsync(int PageIndex, int PageSize);
    }
}
