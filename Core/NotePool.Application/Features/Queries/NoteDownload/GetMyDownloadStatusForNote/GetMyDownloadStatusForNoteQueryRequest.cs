using MediatR;
using System;

namespace NotePool.Application.Features.Queries.Download.GetMyDownloadStatusForNote
{
    public class GetMyDownloadStatusForNoteQueryRequest : IRequest<GetMyDownloadStatusForNoteQueryResponse>
    {
        public Guid NoteId { get; set; }
    }
}
