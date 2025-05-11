using Microsoft.AspNetCore.Mvc;
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

            return Ok(User);
        }

        // api/users/{Id} -> Rotada belirtilen ID'ye sahip kullanıcıyı güncelle
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateUser(long Id, [FromBody] UserUpdateDto Dto)
        {
            bool IsUpdated = await _UserService.UpdateAsync(Id, Dto);

            if (!IsUpdated) return BadRequest();

            return Ok(Dto);
        }

        // api/users/{Id} -> Rotada belirtilen ID'ye sahip kullanıcıyı sil
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteUser(long Id)
        {
            bool IsDeleted = await _UserService.DeleteAsync(Id);
            
            if (!IsDeleted) return NotFound();

            return NoContent();
        }

        // api/users/login -> Kullanıcı hesabına giriş yap
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto Dto)
        {
            var Response = await _UserService.LoginAsync(Dto);

            if (Response == "User not found" || Response == "Wrong password") return BadRequest(Response);

            return Ok(Response);
        }
    }
}
