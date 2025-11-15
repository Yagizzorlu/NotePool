using NotePool.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Domain.Entities
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public string? Code { get; set; }
        public int Year { get; set; }
        public ICollection<Note> Notes { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
    }

}
