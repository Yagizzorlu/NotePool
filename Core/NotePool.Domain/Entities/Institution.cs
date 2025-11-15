using NotePool.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Domain.Entities
{
    public class Institution : BaseEntity
    {
        public string Name { get; set; }
        public string? City { get; set; }
        public ICollection<Department> Departments { get; set; }
    }
}
