using Microsoft.AspNetCore.Identity;
using NotePool.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfileImage { get; set; }
        public Guid InstitutionId { get; set; }
        public Guid DepartmentId { get; set; }
        public Institution Institution { get; set; }
        public Department Department { get; set; }
        public ICollection<Bookmark> Bookmarks { get; set; }
        public ICollection<Note> Notes { get; set; }
        public ICollection<Reaction> Reactions { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<NoteDownload> NoteDownloads { get; set; }
    }
}
