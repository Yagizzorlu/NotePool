using NotePool.Application.Repositories;
using NotePool.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Persistence.Repositories
{
    public class FileWriteRepository : WriteRepository<NotePool.Domain.Entities.File>, IFileWriteRepository
    {
        public FileWriteRepository(NotePoolDbContext context) : base(context)
        {

        }
    
    }
}
