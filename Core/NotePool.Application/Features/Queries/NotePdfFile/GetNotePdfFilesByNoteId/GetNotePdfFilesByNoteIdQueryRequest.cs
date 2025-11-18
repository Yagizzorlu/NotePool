using MediatR;
using System;

namespace NotePool.Application.Features.Queries.NotePdfFile.GetNotePdfFilesByNoteId
{
    public class GetNotePdfFilesByNoteIdQueryRequest : IRequest<GetNotePdfFilesByNoteIdQueryResponse>
    {
        public string NoteId { get; set; }
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 5;
    }
}
