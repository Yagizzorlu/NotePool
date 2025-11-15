using NotePool.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }
        public string? Code { get; set; }
        public Guid InstitutionId { get; set; }
        public Institution Institution { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
