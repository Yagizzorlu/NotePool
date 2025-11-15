using NotePool.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Domain.Entities
{
    public class Note : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int ViewCount { get; set; } = 0;
        public int DownloadCount { get; set; } = 0;
        public string? Tags { get; set; }
        public bool IsApproved { get; set; } = true;
        public Guid DepartmentId { get; set; }
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }
        public Guid InstitutionId { get; set; }
        public Department Department { get; set; }
        public User User { get; set; }
        public Course Course { get; set; }
        public Institution Institution { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Reaction> Reactions { get; set; }
        public ICollection<Bookmark> Bookmarks { get; set; }
        public ICollection<NotePdfFile> NotePdfFiles { get; set; }
    }
}
