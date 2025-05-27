using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moviemo.Dtos;
using Moviemo.Dtos.Comment;
using Moviemo.Services.Interfaces;

namespace Moviemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _CommentService;

        public CommentsController(ICommentService CommentService)
        {
            // Yapıcı metot bağımlılık enjeksiyonu
            _CommentService = CommentService;
        }

        // api/comments -> Tüm yorum bilgilerini al
        [HttpGet]
        public async  Task<IActionResult> GetAllComments([FromQuery] long? MovieId)
        {
            var Comments = new List<CommentGetDto>() { };

            if (MovieId != null)
            {
                Comments = await _CommentService.GetByMovieIdAsync(MovieId);
            } else
            {
                Comments = await _CommentService.GetAllAsync();
            }

            if (Comments == null) 
                return StatusCode(500, "Tüm film bilgileri alınırken bir sunucu hatası meydana geldi.");

            return Ok(Comments);
        }

        // api/comments/{Id} -> Rotada belirtilen ID'ye sahip yorum bilgilerini al
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCommentById(long Id)
        {
            var Comment = await _CommentService.GetByIdAsync(Id);

            if (Comment == null) return NotFound();

            return Ok(Comment);
        }

        // api/comments -> Yorum oluştur
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentCreateDto Dto)
        {
            if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var UserId))
                return Unauthorized("Geçersiz kullanıcı token bilgisi.");

            var Comment = await _CommentService.CreateAsync(Dto, UserId);

            if (Comment == null) return StatusCode(500, "Yorum oluşturulurken bir sunucu hatası meydana geldi");

            return Ok(Comment);
        }

        // api/comments/{Id} -> Rotada belirtilen ID'ye sahip yorumu güncelle
        [Authorize]
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateComment(long Id, [FromBody] CommentUpdateDto Dto)
        {
            if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var UserId))
                return Unauthorized("Geçersiz kullanıcı token bilgisi.");

            var ResponseDto = await _CommentService.UpdateAsync(Id, UserId, Dto);

            if (ResponseDto == null) 
                return StatusCode(500, "Yorum güncellenirken bir sunucu hatası meydana geldi.");

            if (ResponseDto.IsUpdated)
                return Ok(Dto);

            return ResponseDto.Issue switch
            {
                UpdateIssue.NotFound => NotFound($"Comment ID'si {Id} olan comment bulunamadı."),
                UpdateIssue.NotOwner => Unauthorized("Size ait olmayan bir yorumu güncelleyemezsiniz."),
                _ => BadRequest("Yorum güncelleme işlemi gerçekleştirilemedi.")
            };
        }

        // api/comments/{Id} -> Rotada belirtilen ID'ye sahip yorumu sil
        [Authorize]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteComment(long Id)
        {
            if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var UserId))
                return Unauthorized("Geçersiz kullanıcı token bilgisi.");

            var ResponseDto = await _CommentService.DeleteAsync(Id, UserId);

            if (ResponseDto == null)
                return StatusCode(500, "Yorum silinirken bir sunucu hatası meydana geldi.");

            if (ResponseDto.IsDeleted) return NoContent();

            return ResponseDto.Issue switch
            {
                DeleteIssue.NotFound => NotFound("Silinmek istenen yorum bulunamadı."),
                DeleteIssue.NotOwner => Unauthorized("Size ait olmayan bir yorumu silemezsiniz."),
                _ => BadRequest("Yorum silme işlemi gerçekleştirilemedi.")
            };
        }
    }
}
