using MediatR;
using System;

namespace NotePool.Application.Features.Queries.Download.GetNoteDownloadsByNoteId
{
    public class GetNoteDownloadsByNoteIdQueryRequest : IRequest<GetNoteDownloadsByNoteIdQueryResponse>
    {
        public Guid NoteId { get; set; }
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}
