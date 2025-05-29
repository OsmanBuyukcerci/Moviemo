using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<IActionResult> GetAllUsers([FromQuery] string? Username)
        {
            if (Username != null)
            {
                var User = await _UserService.GetByUsernameAsync(Username);

                if (User == null)
                {
                    return StatusCode(500, "Kullanıcı bilgisi alınırken bir sunucu hatası meydana geldi.");
                }

                return Ok(User);
            }

            var Users = await _UserService.GetAllAsync();

            if (Users == null)
                return StatusCode(500, "Tüm kullanıcı bilgileri alınırken bir sunucu hatası meydana geldi.");

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
            var ResponseDto = await _UserService.CreateAsync(Dto);

            if (ResponseDto == null) 
                return StatusCode(500, "Kullanıcı oluşturulurken bir sunucu hatası meydana geldi.");

            if (ResponseDto.IsCreated)
                return Ok(ResponseDto);

            return ResponseDto.Issue switch
            {
                CreateIssue.SameContent => BadRequest("Kullanıcı adı kullanımda."),
                _ => BadRequest("Kullanıcı oluşturma işlemi gerçekleştirilemedi.")
            };
        }

        // api/users/{Id} -> Rotada belirtilen ID'ye sahip kullanıcıyı güncelle
        [Authorize]
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateUser(long Id, [FromBody] UserUpdateDto Dto)
        {
            if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var UserId))
                return Unauthorized("Geçersiz kullanıcı token bilgisi.");

            var ResponseDto = await _UserService.UpdateAsync(Id, UserId, Dto);

            if (ResponseDto == null)
                return StatusCode(500, "Kullanıcı güncellenirken bir sunucu hatası meydana geldi.");

            if (ResponseDto.IsUpdated)
                return Ok(Dto);

            return ResponseDto.Issue switch
            { 
                UpdateIssue.NotFound => NotFound($"User ID'si {Id} olan kullanıcı bulunamadı."),
                UpdateIssue.SameContent => BadRequest("Kullanıcı adı kullanımda."),
                UpdateIssue.NotOwner => Unauthorized("Size ait olmayan bir kullanıcı profilini güncelleyemezsiniz."),
                _ => BadRequest("Kullanıcı güncelleme işlemi gerçekleştirilemedi.")
            };
        }

        // api/users/{Id} -> Rotada belirtilen ID'ye sahip kullanıcıyı sil
        [Authorize]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(long Id)
        {
            if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var UserId))
                return Unauthorized("Geçersiz kullanıcı token bilgisi.");

            var ResponseDto = await _UserService.DeleteAsync(Id, UserId);

            if (ResponseDto == null)
                return StatusCode(500, "Kullanıcı silinirken bir sunucu hatası meydana geldi.");

            if (ResponseDto.IsDeleted) return NoContent();

            return ResponseDto.Issue switch
            { 
                DeleteIssue.NotFound => NotFound($"User ID'si {Id} olan kullanıcı bulunamadı."),
                DeleteIssue.NotOwner => Unauthorized("Size ait olmayan bir kullanıcı profilini silemezsiniz."),
                _ => BadRequest("Kullanıcı silme işlemi gerçekleştirilemedi.")
            };
        }

        // api/users/login -> Kullanıcı hesabına giriş yap
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto Dto)
        {
            var ResponseDto = await _UserService.LoginAsync(Dto);

            if (ResponseDto == null) 
                return StatusCode(500, "Kullanıcı girişi sırasında bir sunucu hatası meydana geldi.");

            if (ResponseDto.Issue == LoginIssue.None)
                return Ok(ResponseDto);

            return ResponseDto.Issue switch
            {
                LoginIssue.NotFound => NotFound("Girilen kullanıcı adına sahip bir hesap bulunamadı."),
                LoginIssue.IncorrectPassword => BadRequest("Parola hatalı."),
                _ => BadRequest("Kullanıcı girişi gerçekleştirilemedi.")
            };
        }

        // api/users/refresh-token -> Kullanıcının access ve refresh tokenlerini yenile
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokens([FromBody] RefreshTokenRequestDto Dto)
        {
            if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var UserId))
                return Unauthorized("Geçersiz kullanıcı token bilgisi.");

            var Result = await _UserService.RefreshTokensAsync(UserId, Dto);

            if (Result == null)
                return StatusCode(500, "Token yenilenirken bir sunucu hatası meydana geldi.");

            if (Result.AccessToken == null || Result.RefreshToken == null) 
                return Unauthorized("Geçersiz refresh token");

            return Ok(Result);
        }

        [Authorize]
        [HttpPut("{Id}/change-password")]
        public async Task<IActionResult> ChangePassword(long Id, [FromBody] ChangePasswordDto Dto)
        {
            if (!long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var UserId))
                return Unauthorized("Geçersiz kullanıcı token bilgisi.");

            var ResponseDto = await _UserService.ChangePasswordAsync(Dto, Id, UserId);

            if (ResponseDto == null)
                return StatusCode(500, "Kullanıcı parolası değiştirilirken bir sunucu hatası meydana geldi.");

            return ResponseDto.Issue switch
            {
                PasswordChangeIssue.None => Ok(ResponseDto),
                PasswordChangeIssue.IncorrectOldPassword => BadRequest("Eski parola hatalı girildi."),
                _ => BadRequest("Parola değiştirme işlemi gerçekleştirilmedi.")
            };
        }
    }
}
