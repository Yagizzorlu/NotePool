using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Domain.Entities
{
    public class NotePdfFile : File
    {
        public Guid NoteId { get; set; }
        public Note Note { get; set; }

    }
}
