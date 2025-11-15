using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.ViewModels.Notes
{
    public class VM_Update_Note
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }
        public string CourseId { get; set; }
        public string InstitutionId { get; set; }
        public string UserId { get; set; }
    }
}
