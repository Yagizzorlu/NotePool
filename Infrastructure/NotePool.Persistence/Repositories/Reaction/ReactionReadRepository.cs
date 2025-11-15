using NotePool.Application.Repositories;
using NotePool.Domain.Entities;
using NotePool.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Persistence.Repositories
{
    public class ReactionReadRepository : ReadRepository<Reaction>, IReactionReadRepository
    {
        public ReactionReadRepository(NotePoolDbContext context) : base(context)
        {
        }
    }
}
