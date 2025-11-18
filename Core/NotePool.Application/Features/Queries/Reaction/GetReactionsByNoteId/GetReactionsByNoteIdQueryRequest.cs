using MediatR;
using NotePool.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Reaction.GetReactionsByNoteId
{
    public class GetReactionsByNoteIdQueryRequest : IRequest<GetReactionsByNoteIdQueryResponse>
    {
        public string NoteId { get; set; }
        public ReactionType Type { get; set; }
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 15;
    }
}
