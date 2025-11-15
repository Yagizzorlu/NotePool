using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Note.GetByDepartmentId
{
    public class GetByDepartmentIdQueryResponse
    {
        public object Notes { get; set; }
        public int TotalCount { get; set; }
    }
}