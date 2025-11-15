using NotePool.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Domain.Entities
{
    public class Reaction : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid NoteId { get; set; }
        public short Value { get; set; }
        public User User { get; set; }
        public Note Note { get; set; }
    }
}
