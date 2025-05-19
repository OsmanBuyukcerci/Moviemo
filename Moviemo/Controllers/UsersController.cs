using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moviemo.Dtos;
using Moviemo.Dtos.Token;
using Moviemo.Dtos.User;
using Moviemo.Services.Interfaces;

namespace Moviemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _UserService;

        public UsersController(IUserService UserService)
        {
            // Yapıcı metot içinde bağımlılık enjeksiyonu
            _UserService = UserService;
        }

        // api/users -> Tüm kullanıcı bilgilerini al
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var Users = await _UserService.GetAllAsync();

            return Ok(Users);
        }

        // api/users/{Id} -> Rotada belirtilen ID'ye sahip kullanıcı bilgilerini al
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUserById(long Id)
        {
            var User = await _UserService.GetByIdAsync(Id);

            if (User == null) return NotFound();

            return Ok(User);
        }

        // api/users -> Kullanıcı oluştur
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto Dto)
        {
            var User = await _UserService.CreateAsync(Dto);

            if (User == null) return BadRequest("Kullanıcı adı kullanımda");

            return Ok(User);
        }

        // api/users/{Id} -> Rotada belirtilen ID'ye sahip kullanıcıyı güncelle
        [Authorize]
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateUser(long Id, [FromBody] UserUpdateDto Dto)
        {
            var UserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var ResponseDto = await _UserService.UpdateAsync(Id, UserId, Dto);

            if (ResponseDto.IsUpdated) Ok(Dto);

            else if (ResponseDto.Issue == UpdateIssue.NotFound)
                return NotFound($"User ID'si {Id} olan kullanıcı bulunamadı.");

            else if (ResponseDto.Issue == UpdateIssue.Unauthorized)
                return Unauthorized("Başka bir kullanıcının bilgilerini güncelleyemezsiniz.");

            else if (ResponseDto.Issue == UpdateIssue.SameUsername)
                return BadRequest("Kullanıcı adı kullanımda.");

            return BadRequest("Kullanıcı güncelleme işlemi gerçekleştirilemedi.");
        }

        // api/users/{Id} -> Rotada belirtilen ID'ye sahip kullanıcıyı sil
        [Authorize]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(long Id)
        {
            var UserId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var ResponseDto = await _UserService.DeleteAsync(Id, UserId);

            if (ResponseDto.IsDeleted) return NoContent();

            else if (ResponseDto.Issue == DeleteIssue.NotFound)
                return NotFound($"User ID'si {Id} olan kullanıcı bulunamadı.");

            else if (ResponseDto.Issue == DeleteIssue.Unauthorized)
                return Unauthorized("Size ait olmayan bir kullanıcı hesabını silemezsiniz.");

            return BadRequest("Kullanıcı silme işlemi gerçekleştirilemedi.");
        }

        // api/users/login -> Kullanıcı hesabına giriş yap
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto Dto)
        {
            var Response = await _UserService.LoginAsync(Dto);

            if (Response == null) return BadRequest("Kullanıcı adı veya parola hatalı");

            return Ok(Response);
        }

        // api/users/refresh-token -> Kullanıcının access ve refresh tokenlerini yenile
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto Dto)
        {
            var Result = await _UserService.RefreshTokenAsync(Dto);

            if (Result == null || Result.AccessToken == null || Result.RefreshToken == null) 
                return Unauthorized("Geçersiz refresh token");

            return Ok(Result);
        }
    }
}
