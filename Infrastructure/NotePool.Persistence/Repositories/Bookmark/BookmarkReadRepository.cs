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
    public class BookmarkReadRepository : ReadRepository<Bookmark>, IBookmarkReadRepository
    {
        public BookmarkReadRepository(NotePoolDbContext context) : base(context)
        {
        }
    }
}
