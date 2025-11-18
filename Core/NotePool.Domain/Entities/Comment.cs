using NotePool.Domain.Entities.Common;
using System;
using System.Collections.Generic;

namespace NotePool.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid NoteId { get; set; }
        public string Content { get; set; }
        public Guid? ParentId { get; set; }
        public Comment Parent { get; set; }
        public ICollection<Comment> Replies { get; set; }
        public User User { get; set; }
        public Note Note { get; set; }
    }
}

