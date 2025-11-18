using MediatR;
using NotePool.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Reaction.SetReaction
{
    public class SetReactionCommandRequest : IRequest<SetReactionCommandResponse>
    {
        public string NoteId { get; set; }
        public ReactionType Type { get; set; }
    }
}
