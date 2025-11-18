using MediatR;
using NotePool.Application.Features.Queries.Note.GetByUserId;
using System;

namespace NotePool.Application.Features.Queries.Note.GetNotesByUserId
{
    public class GetByUserIdQueryRequest : IRequest<GetByUserIdQueryResponse>
    {
        public string Id { get; set; }
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}
