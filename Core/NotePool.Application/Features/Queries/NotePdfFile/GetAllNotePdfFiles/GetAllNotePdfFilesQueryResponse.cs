using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.NotePdfFile.GetAllNotePdfFiles
{
    public class GetAllNotePdfFilesQueryResponse
    {
        public object Files { get; set; }
        public int TotalCount { get; set; }
    }
}
