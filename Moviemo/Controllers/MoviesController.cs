using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moviemo.Dtos;
using Moviemo.Dtos.Movie;
using Moviemo.Services.Interfaces;

namespace Moviemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _MovieService;

        public MoviesController(IMovieService MovieService)
        {
            // Yapıcı metot içerisinde bağımlılık enjeksiyonu
            _MovieService = MovieService;
        }

        // api/movies -> Tüm film bilgilerini al
        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            var Movies = await _MovieService.GetAllAsync();

            return Ok(Movies);
        }

        // api/movies/{Id} -> Rotada belirtilen ID'ye sahip film bilgisini al
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetMovieById(long Id)
        {
            var Movie = await _MovieService.GetByIdAsync(Id);

            if (Movie == null) return NotFound();

            return Ok(Movie);
        }

        // api/movies -> Film oluştur
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieCreateDto Dto)
        {
            var Movie = await _MovieService.CreateAsync(Dto);

            return Ok(Movie);
        }

        // api/movies/{Id} -> Rotada belirtilen ID'ye sahip filmi güncelle
        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateMovie(long Id, MovieUpdateDto Dto)
        {
            var ResponseDto = await _MovieService.UpdateAsync(Id, Dto);

            if (ResponseDto.IsUpdated) return Ok(Dto);

            else if (ResponseDto.Issue == UpdateIssue.NotFound) 
                return NotFound($"Movie ID'si {Id} olan film bulunamadı.");

            return BadRequest("Film güncelleştirme işlemi gerçekleştirilemedi.");
        }

        // api/movies/{Id} -> Rotada belirtilen ID'ye sahip filmi sil
        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteMovie(long Id)
        {
            var ResponseDto = await _MovieService.DeleteAsync(Id);

            if (ResponseDto.IsDeleted) return NoContent();

            else if (ResponseDto.Issue == DeleteIssue.NotFound)
                return NotFound($"Movie ID'si {Id} olan film bulunamadı");

            return BadRequest("Film silme işlemi gerçekleştirilemedi.");
        }
    }
}
