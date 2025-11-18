using MediatR;
using NotePool.Domain.Entities.Enums;
using NotePool.Application.Features.Queries.Reaction.GetMyReactionForNote;
using System;

namespace NotePool.Application.Features.Queries.Reaction.GetMyReactionForNote
{
    public class GetMyReactionForNoteQueryRequest : IRequest<GetMyReactionForNoteQueryResponse>
    {
        public string NoteId { get; set; }
    }
}