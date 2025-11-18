using NotePool.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Reaction.SetReaction
{
    public class SetReactionCommandResponse
    {
        public int NewLikeCount { get; set; }
        public int NewDislikeCount { get; set; }
        public ReactionType? CurrentUserReaction { get; set; }
    }
}
