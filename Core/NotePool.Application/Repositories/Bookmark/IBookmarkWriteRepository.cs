using NotePool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Repositories
{
    public interface IBookmarkWriteRepository : IWriteRepository<Bookmark>
    {
    }
}
