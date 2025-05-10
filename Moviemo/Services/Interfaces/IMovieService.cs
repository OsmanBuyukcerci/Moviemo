using Moviemo.Dtos.Movie;
using Moviemo.Models;

namespace Moviemo.Services.Interfaces
{
    public interface IMovieService
    {
        Task<List<MovieGetDto>> GetAllAsync();
        Task<MovieGetDto?> GetByIdAsync(long Id);
        Task<MovieCreateDto> CreateAsync(MovieCreateDto Dto);
        Task<bool> UpdateAsync(long Id, MovieUpdateDto Dto);
        Task<bool> DeleteAsync(long Id);
    }
}
