using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.NotePdfFile.GetNotePdfFileById
{
    public class GetNotePdfFileByIdQueryResponse
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Storage { get; set; }
        public Guid NoteId { get; set; }
    }
}
