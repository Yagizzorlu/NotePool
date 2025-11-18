using System;
using MediatR;
using NotePool.Application.Features.Queries.Bookmark.GetBookmarkCountByNoteId;

namespace NotePool.Application.Features.Queries.Bookmark.GetBookmarkCountByNoteId
{
    public class GetBookmarkCountByNoteIdQueryRequest : IRequest<GetBookmarkCountByNoteIdQueryResponse>
    {
        public string NoteId { get; set; }
    }
}
