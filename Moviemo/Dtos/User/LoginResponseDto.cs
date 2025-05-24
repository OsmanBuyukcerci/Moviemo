namespace Moviemo.Dtos.User
{
    public class LoginResponseDto
    {
        public bool IsLoggedIn { get; set; } = false;
        public LoginIssue Issue { get; set; } = LoginIssue.None;
    }

    public enum LoginIssue
    {
        None,
        NotFound,
        IncorrectPassword
    }
}
