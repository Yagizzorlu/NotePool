using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Course.GetAllCourse
{
    public class GetAllCourseQueryResponse
    {
        public int TotalCount { get; set; }
        public object Courses { get; set; }
    }
}
