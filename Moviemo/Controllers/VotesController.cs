using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetAllVotes()
        {
            var Votes = await _VoteService.GetAllAsync();

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
        [HttpPost]
        public async Task<IActionResult> CreateVote([FromBody] VoteCreateDto Dto)
        {
            var Vote = await _VoteService.CreateAsync(Dto);

            return Ok(Dto);
        }

        // api/votes/{Id} -> Rotada belirtilen ID'ye sahip oy bilgisini güncelle
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateVote(long Id, VoteUpdateDto Dto)
        {
            bool IsUpdated = await _VoteService.UpdateAsync(Id, Dto);

            if (!IsUpdated) return BadRequest();

            return Ok(Dto);
        }

        // api/votes/{Id} -> Rotada belirtilen ID'ye sahip oyu sil}
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteVote(long Id)
        {
            bool IsDeleted = await _VoteService.DeleteAsync(Id);

            if (!IsDeleted) return NotFound();

            return NoContent();
        }
    }
}
