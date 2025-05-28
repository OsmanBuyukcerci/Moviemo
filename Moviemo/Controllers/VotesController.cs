using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moviemo.Dtos;
using Moviemo.Dtos.Vote;
using Moviemo.Services.Interfaces;

namespace Moviemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly IVoteService _VoteService;

        public VotesController(IVoteService VoteService)
        {
            _VoteService = VoteService;
        }

        // api/votes -> Tüm oy bilgilerini al
        [HttpGet]
        public async Task<IActionResult> GetAllVotes([FromQuery] long? UserId, [FromQuery] long? CommentId)
        {
            if (UserId != null && CommentId != null)
            {
                var Vote = await _VoteService.GetByUserAndCommentIdAsync(UserId, CommentId);

                if (Vote == null)
                    return Ok(new {id=-1, voteType=-2});

                return Ok(Vote);
            }

            var Votes = await _VoteService.GetAllAsync();

            if (Votes == null)
                return StatusCode(500, "Tüm oy bilgileri alınırken bir sunucu hatası meydana geldi.");

            return Ok(Votes);
        }

        // api/votes/{Id} -> Rotada belirtilen ID'ye sahip oy bilgisini al
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetVoteById(long Id)
        {
            var Vote = await _VoteService.GetByIdAsync(Id);

            if (Vote == null) return NotFound();

            return Ok(Vote);
        }

        // api/votes -> Oy oluştur
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateVote([FromBody] VoteCreateDto Dto)
        {
            if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var UserId))
                return Unauthorized("Geçersiz kullanıcı token bilgisi.");

            var ResponseDto = await _VoteService.CreateAsync(Dto, UserId);

            if (ResponseDto == null)
                return StatusCode(500, "Oy oluşturulurken bir sunucu hatası meydana geldi.");

            if (ResponseDto.IsCreated == true)
                return Ok(Dto);

            return ResponseDto.Issue switch
            {
                CreateIssue.SameContent => Conflict("Aynı yoruma birden fazla oy atamazsınız."),
                _ => BadRequest("Oy oluşturma işlemi gerçekleştirilemedi.")
            };
        }

        // api/votes/{Id} -> Rotada belirtilen ID'ye sahip oy bilgisini güncelle
        [Authorize]
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateVote(long Id, VoteUpdateDto Dto)
        {
            if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var UserId))
                return Unauthorized("Geçersiz kullanıcı token bilgisi.");

            var ResponseDto = await _VoteService.UpdateAsync(Id, UserId, Dto);

            if (ResponseDto == null)
                return StatusCode(500, "Oy bilgisi güncellenirken bir sunucu hatası meydana geldi.");

            if (ResponseDto.IsUpdated) return Ok(Dto);

            return ResponseDto.Issue switch
            {
                UpdateIssue.NotFound => NotFound($"Vote ID'si {Id} olan oy bulunamadı."),
                UpdateIssue.NotOwner => Unauthorized("Size ait olmayan bir oyu güncelleyemezsiniz."),
                _ => BadRequest("Oy güncelleme işlemi gerçekleştirilemedi.")
            };
        }

        // api/votes/{Id} -> Rotada belirtilen ID'ye sahip oyu sil}
        [Authorize]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteVote(long Id)
        {
            if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var UserId))
                return Unauthorized("Geçersiz kullanıcı token bilgisi.");

            var ResponseDto = await _VoteService.DeleteAsync(Id, UserId);

            if (ResponseDto == null)
                return StatusCode(500, "Oy silinirken bir sunucu hatası meydana geldi.");

            if (ResponseDto.IsDeleted) return NoContent();

            return ResponseDto.Issue switch
            {
                DeleteIssue.NotFound => NotFound($"Vote ID'si {Id} olan oy bulunamadı."),
                DeleteIssue.NotOwner => Unauthorized("Size ait olmayan bir oyu silemezsiniz."),
                _ => BadRequest("Oy silme işlemi gerçekleştirilemedi.")
            };
        }
    }
}
