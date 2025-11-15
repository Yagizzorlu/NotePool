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
    public class ReactionWriteRepository : WriteRepository<Reaction>, IReactionWriteRepository
    {
        public ReactionWriteRepository(NotePoolDbContext context) : base(context)
        {
        }
    }
}
