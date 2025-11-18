namespace NotePool.Application.Features.Queries.Comment.GetCommentsByNoteId
{
    public class GetCommentsByNoteIdQueryResponse
    {
        public object Comments { get; set; }
        public int TotalCount { get; set; }
    }
}
