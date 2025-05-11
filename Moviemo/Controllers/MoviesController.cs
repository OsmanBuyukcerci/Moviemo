using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieCreateDto Dto)
        {
            var Movie = await _MovieService.CreateAsync(Dto);

            return Ok(Movie);
        }

        // api/movies/{Id} -> Rotada belirtilen ID'ye sahip filmi güncelle
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateMovie(long Id, MovieUpdateDto Dto)
        {
            bool IsUpdated = await _MovieService.UpdateAsync(Id, Dto);

            if (!IsUpdated) return BadRequest();

            return Ok(Dto);
        }

        // api/movies/{Id} -> Rotada belirtilen ID'ye sahip filmi sil
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteMovie(long Id)
        {
            bool IsDeleted = await _MovieService.DeleteAsync(Id);

            if (!IsDeleted) return NotFound();

            return NoContent();
        }
    }
}
