namespace Moviemo.Dtos
{
    public class CreateResponseDto
    {
        public bool IsCreated { get; set; }
        public CreateIssue Issue { get; set; } = CreateIssue.None;
    }

    public enum CreateIssue
    {
        None,
        SameContent
    }
}
