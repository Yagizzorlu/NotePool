using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.ViewModels.Notes
{
    public class VM_Create_Note
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }
        public Guid CourseId { get; set; }
        public Guid InstitutionId { get; set; }
        public Guid UserId { get; set; }
    }
}
