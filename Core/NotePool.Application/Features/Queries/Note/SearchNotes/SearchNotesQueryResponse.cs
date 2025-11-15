using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Note.SearchNotes
{
    public class SearchNotesQueryResponse
    {
        public object Notes { get; set; }
        public int TotalCount { get; set; }
    }
}
