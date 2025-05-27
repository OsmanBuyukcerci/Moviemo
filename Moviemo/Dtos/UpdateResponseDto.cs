namespace Moviemo.Dtos
{
    public class UpdateResponseDto
    {
        public bool IsUpdated { get; set; }
        public UpdateIssue Issue {  get; set; } = UpdateIssue.None;
    }

    public enum UpdateIssue
    {
        None,
        NotFound,
        SameContent,
        NotOwner
    }
}
