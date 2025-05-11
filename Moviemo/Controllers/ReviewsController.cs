using Microsoft.AspNetCore.Mvc;
using Moviemo.Dtos.Review;
using Moviemo.Services.Interfaces;

namespace Moviemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _ReviewService;

        public ReviewsController(IReviewService ReviewService)
        {
            // Yapıcı metot bağımlılık enjeksiyonu
            _ReviewService = ReviewService;
        }

        // api/reviews -> Tüm inceleme bilgilerini al
        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var Reviews = await _ReviewService.GetAllAsync();

            return Ok(Reviews);
        }

        // api/reviews/{Id} -> Rotada belirtilen ID'ye sahip inceleme bilgilerini al
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetReviewById(long Id)
        {
            var Review = await _ReviewService.GetByIdAsync(Id);

            if (Review == null) return NotFound();

            return Ok(Review);
        }

        // api/reviews -> İnceleme oluştur
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDto Dto)
        {
            ReviewCreateDto ResponseDto = await _ReviewService.CreateAsync(Dto);

            return Ok(ResponseDto);
        }

        // api/reviews/{Id} -> Rotada belirtilen ID'ye sahip incelemeyi güncelle
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateReview(long Id, [FromBody] ReviewUpdateDto Dto)
        {
            bool IsUpdated = await _ReviewService.UpdateAsync(Id, Dto);

            if (!IsUpdated) return BadRequest();

            return Ok(Dto);
        }

        // api/reviews/{Id} -> Rotada belirtilen ID'ye sahip incelemeyi sil
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteReview(long Id)
        {
            bool IsDeleted = await _ReviewService.DeleteAsync(Id);

            if (!IsDeleted) return BadRequest();

            return NoContent();
        }
    }
}
