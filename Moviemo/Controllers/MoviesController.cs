using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<IActionResult> GetAllMovies([FromQuery] int? PageIndex, [FromQuery] int? PageSize)
        {
            if (PageIndex != null && PageSize != null)
            {
                var Response = await _MovieService.GetByPageSizeAsync(PageIndex ?? -1, PageSize ?? -1);

                if (Response == null)
                {
                    return StatusCode(500, "Sayfa bilgisi alınırken bir sunucu hatası meydana geldi.");
                }

                return Ok(Response);
            }

            var Movies = await _MovieService.GetAllAsync();

            if (Movies == null)
                return StatusCode(500, "Tüm film bilgileri alınırken bir sunucu hatası meydana geldi.");

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

            if (Movie == null)
                return StatusCode(500, "Film oluşturulurken bir sunucu hatası meydana geldi.");

            return Ok(Movie);
        }

        // api/movies/{Id} -> Rotada belirtilen ID'ye sahip filmi güncelle
        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateMovie(long Id, MovieUpdateDto Dto)
        {
            var ResponseDto = await _MovieService.UpdateAsync(Id, Dto);

            if (ResponseDto == null)
                return StatusCode(500, "Yorum güncellenirken bir sunucu hatası meydana geldi.");

            if (ResponseDto.IsUpdated)
                return Ok(Dto);

            return ResponseDto.Issue switch
            {
                UpdateIssue.NotFound => NotFound($"Movie ID'si {Id} olan film bulunamadı."),
                _ => BadRequest("Film güncelleştirme işlemi gerçekleştirilemedi.")
            };
        }

        // api/movies/{Id} -> Rotada belirtilen ID'ye sahip filmi sil
        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteMovie(long Id)
        {
            var ResponseDto = await _MovieService.DeleteAsync(Id);

            if (ResponseDto == null)
                return StatusCode(500, "Film silinirken bir sunucu hatası meydana geldi");

            if (ResponseDto.IsDeleted) return NoContent();

            return ResponseDto.Issue switch
            {
                DeleteIssue.NotFound => NotFound($"Movie ID'si {Id} olan film bulunamadı"),
                _ => BadRequest("Film silme işlemi gerçekleştirilemedi.")
            };
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMovie([FromQuery] string Query)
        {
            var Results = await _MovieService.SearchAsync(Query);

            if (Results == null)
            {
                return StatusCode(500, "Arama yapılırken bir sunucu hatası meydana geldi.");
            }

            return Ok(Results);
        }
    }
}
