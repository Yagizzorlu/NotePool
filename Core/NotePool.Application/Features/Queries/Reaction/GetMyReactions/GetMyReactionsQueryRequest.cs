using MediatR;
using NotePool.Domain.Entities.Enums;
using NotePool.Application.Features.Queries.Reaction.GetMyReactions;
using System;

namespace NotePool.Application.Features.Queries.Reaction.GetMyReactions
{
    public class GetMyReactionsQueryRequest : IRequest<GetMyReactionsQueryResponse>
    {
        public ReactionType Type { get; set; } = ReactionType.Like;
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}
