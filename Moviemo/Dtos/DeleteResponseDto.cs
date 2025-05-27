namespace Moviemo.Dtos
{
    public class DeleteResponseDto
    {
        public bool IsDeleted { get; set; }
        public DeleteIssue Issue { get; set; }  = DeleteIssue.None;
    }

    public enum DeleteIssue
    {
        None,
        NotFound,
        NotOwner
    }
}
