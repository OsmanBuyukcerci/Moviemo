namespace Moviemo.Dtos.User
{
    public class PasswordChangeResponseDto
    {
        public PasswordChangeIssue Issue { get; set; } = PasswordChangeIssue.None;
    }

    public enum PasswordChangeIssue
    {
        None,
        IncorrectOldPassword,
        Unauthorized
    }
}
