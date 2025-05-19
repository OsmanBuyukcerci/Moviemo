namespace Moviemo.Dtos
{
    public class UpdateResponseDto
    {
        public bool IsUpdated { get; set; }
        public UpdateIssue Issue {  get; set; }
    }

    public enum UpdateIssue
    {
        None,
        NotFound,
        Unauthorized,
        SameUsername
    }
}
