namespace NotePool.Application.Features.Commands.Comment.CreateComment
{
    public class CreateCommentCommandResponse
    {
        public bool Succeeded { get; set; }
        public object CreatedComment { get; set; }
    }
}
