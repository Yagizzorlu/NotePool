using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Reaction.GetReactionsByNoteId
{
    public class GetReactionsByNoteIdQueryResponse
    {
        public object Users { get; set; }
        public int TotalCount { get; set; }
    }
}
