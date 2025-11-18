using MediatR;
using NotePool.Domain.Entities.Enums;
using NotePool.Application.Features.Queries.Reaction.GetReactionsByUserId;
using System;

namespace NotePool.Application.Features.Queries.Reaction.GetReactionsByUserId
{
    public class GetReactionsByUserIdQueryRequest : IRequest<GetReactionsByUserIdQueryResponse>
    {
        public string Id { get; set; }
        public ReactionType Type { get; set; } = ReactionType.Like;
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}