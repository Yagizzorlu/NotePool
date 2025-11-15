using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Course.GetByIdCourse
{
    public class GetByIdCourseQueryResponse
    {
            public string Name { get; set; }
            public string? Code { get; set; }
            public int Year { get; set; }
            public Guid DepartmentId { get; set; }
            public object Notes { get; set; }
    }
}
