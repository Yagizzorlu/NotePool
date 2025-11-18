using NotePool.Domain.Entities.Enums;
using System;

namespace NotePool.Application.Features.Queries.Reaction.GetMyReactionForNote
{
    public class GetMyReactionForNoteQueryResponse
    {
        public ReactionType? UserReaction { get; set; }
    }
}
