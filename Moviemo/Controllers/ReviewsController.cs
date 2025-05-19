using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moviemo.Dtos;
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
        [Authorize]
        public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDto Dto)
        {
            ReviewCreateDto ResponseDto = await _ReviewService.CreateAsync(Dto);

            return Ok(ResponseDto);
        }

        // api/reviews/{Id} -> Rotada belirtilen ID'ye sahip incelemeyi güncelle
        [Authorize]
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateReview(long Id, [FromBody] ReviewUpdateDto Dto)
        {
            var UserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var ResponseDto = await _ReviewService.UpdateAsync(Id, UserId, Dto);

            if (ResponseDto.IsUpdated) return Ok(Dto);

            else if (ResponseDto.Issue == UpdateIssue.NotFound)
                return NotFound($"Review ID'si {Id} olan inceleme bulunamadı.");

            else if (ResponseDto.Issue == UpdateIssue.Unauthorized)
                return Unauthorized("Size ait olmayan bir incelemeyi güncelleyemezsiniz.");

            return BadRequest("İnceleme güncelleme işlemi gerçekleştirilemedi.");
        }

        // api/reviews/{Id} -> Rotada belirtilen ID'ye sahip incelemeyi sil
        [Authorize]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteReview(long Id)
        {
            var UserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var ResponseDto = await _ReviewService.DeleteAsync(Id, UserId);

            if (ResponseDto.IsDeleted) return NoContent();

            else if (ResponseDto.Issue == DeleteIssue.NotFound)
                return NotFound($"Review ID'si {Id} olan inceleme bulunamadı.");

            else if (ResponseDto.Issue == DeleteIssue.Unauthorized)
                return Unauthorized("Size ait olmayan bir incelemeyi silemezsiniz.");

            return BadRequest("İnceleme silme işlemi gerçekleştirilemedi.");
        }
    }
}
