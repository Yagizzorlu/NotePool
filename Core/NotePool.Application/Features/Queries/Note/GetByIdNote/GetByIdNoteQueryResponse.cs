using NotePool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Note.GetByIdNote
{
    public class GetByIdNoteQueryResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }
        public DateTime CreatedDate { get; set; }

        public int ViewCount { get; set; }
        public int DownloadCount { get; set; }
        public int CommentCount { get; set; }
        public int LikeCount { get; set; }

        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }

        public object PdfFiles { get; set; }
        public object Comments { get; set; }
    }
}
