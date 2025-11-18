using MediatR;
using System;

namespace NotePool.Application.Features.Queries.Download.GetNoteDownloadsByUserId
{
    public class GetNoteDownloadsByUserIdQueryRequest : IRequest<GetNoteDownloadsByUserIdQueryResponse>
    {
        public Guid UserId { get; set; }
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}
