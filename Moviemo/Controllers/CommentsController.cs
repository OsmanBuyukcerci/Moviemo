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
        public async  Task<IActionResult> GetAllComments()
        {
            var Users = await _CommentService.GetAllAsync();

            return Ok(Users);
        }

        // api/comments/{Id} -> Rotada belirtilen ID'ye sahip yorum bilgilerini al
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCommentById(long Id)
        {
            var User = await _CommentService.GetByIdAsync(Id);

            if (User == null) return NotFound();

            return Ok(User);
        }

        // api/comments -> Yorum oluştur
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentCreateDto Dto)
        {
            var Comment = await _CommentService.CreateAsync(Dto);

            return Ok(Comment);
        }

        // api/comments/{Id} -> Rotada belirtilen ID'ye sahip yorumu güncelle
        [Authorize]
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateComment(long Id, [FromBody] CommentUpdateDto Dto)
        {
            var UserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var ResponseDto = await _CommentService.UpdateAsync(Id, UserId, Dto);

            if (ResponseDto.IsUpdated) return Ok(Dto);

            else if (ResponseDto.Issue == UpdateIssue.NotFound)
                return NotFound($"Comment ID'si {Id} olan comment bulunamadı");

            else if (ResponseDto.Issue == UpdateIssue.Unauthorized)
                return Unauthorized("Size ait olmayan bir yorumu güncelleyemezsiniz");

                return BadRequest("Yorum güncelleme işlemi gerçekleştirilemedi.");
        }

        // api/comments/{Id} -> Rotada belirtilen ID'ye sahip yorumu sil
        [Authorize]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteComment(long Id)
        {
            var UserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var ResponseDto = await _CommentService.DeleteAsync(Id, UserId);

            if (ResponseDto.IsDeleted) return NoContent();

            else if (ResponseDto.Issue == DeleteIssue.NotFound) return NotFound("Silinmek istenen yorum bulunamadı.");

            else if (ResponseDto.Issue == DeleteIssue.Unauthorized)
                return Unauthorized("Size ait olmayan bir yorumu silemezsiniz.");

            return BadRequest("Yorum silme işlemi gerçekleştirilemedi.");
        }
    }
}
