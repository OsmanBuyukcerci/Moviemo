using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentCreateDto Dto)
        {
            var Comment = await _CommentService.CreateAsync(Dto);

            return Ok(Comment);
        }

        // api/comments/{Id} -> Rotada belirtilen ID'ye sahip yorumu güncelle
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateComment(long Id, [FromBody] CommentUpdateDto Dto)
        {
            var IsUpdated = await _CommentService.UpdateAsync(Id, Dto);

            if (!IsUpdated) return BadRequest();

            return Ok(Dto);
        }

        // api/comments/{Id} -> Rotada belirtilen ID'ye sahip yorumu sil
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteComment(long Id)
        {
            var IsDeleted = await _CommentService.DeleteAsync(Id);

            if (!IsDeleted) return NotFound();

            return NoContent();
        }
    }
}
