using NotePool.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Domain.Entities
{
    public class Bookmark : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid NoteId { get; set; }
        public Note Note { get; set; }
        public User User { get; set; }
    }
}
