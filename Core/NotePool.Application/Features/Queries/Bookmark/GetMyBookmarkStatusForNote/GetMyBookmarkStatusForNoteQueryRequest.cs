using MediatR;
using NotePool.Application.Features.Queries.Bookmark.GetMyBookmarkStatusForNote;

namespace NotePool.Application.Features.Queries.Bookmark.GetMyBookmarkStatusForNote
{
    public class GetMyBookmarkStatusForNoteQueryRequest : IRequest<GetMyBookmarkStatusForNoteQueryResponse>
    {
        public string NoteId { get; set; }
    }
}
