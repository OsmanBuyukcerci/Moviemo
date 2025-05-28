using Moviemo.Dtos.Token;

namespace Moviemo.Dtos.User
{
    public class LoginResponseDto
    {
        public string Username {  get; set; } = string.Empty;
        public bool IsLoggedIn { get; set; } = false;
        public LoginIssue Issue { get; set; } = LoginIssue.None;
        public TokenResponseDto TokenResponse { get; set; }
    }

    public enum LoginIssue
    {
        None,
        NotFound,
        IncorrectPassword
    }
}
