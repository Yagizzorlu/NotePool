using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Course.GetByDepartment
{
    public class GetByDepartmentQueryResponse
    {
        public object Courses { get; set; }
        public int TotalCount { get; set; }
    }
}
