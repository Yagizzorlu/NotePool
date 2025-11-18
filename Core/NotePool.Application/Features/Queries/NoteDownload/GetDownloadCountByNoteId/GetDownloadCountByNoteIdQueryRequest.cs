using MediatR;
using System;

namespace NotePool.Application.Features.Queries.Download.GetDownloadCountByNoteId
{
    public class GetDownloadCountByNoteIdQueryRequest : IRequest<GetDownloadCountByNoteIdQueryResponse>
    {
        public Guid NoteId { get; set; }
    }
}
