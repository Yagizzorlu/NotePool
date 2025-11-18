using MediatR;
using NotePool.Application.Features.Queries.Comment.GetCommentsByNoteId;
using System;

namespace NotePool.Application.Features.Queries.Comment.GetCommentsByNoteId
{
    public class GetCommentsByNoteIdQueryRequest : IRequest<GetCommentsByNoteIdQueryResponse>
    {
        public string NoteId { get; set; }
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}
