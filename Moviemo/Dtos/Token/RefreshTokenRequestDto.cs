namespace Moviemo.Dtos.Token
{
    public class RefreshTokenRequestDto
    {
        public long UserId {  get; set; }
        public string RefreshToken { get; set; } = String.Empty;
    }
}
