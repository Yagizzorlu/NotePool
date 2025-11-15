using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Repositories
{
    public interface IFileWriteRepository : IWriteRepository<NotePool.Domain.Entities.File>
    {
    }
}
