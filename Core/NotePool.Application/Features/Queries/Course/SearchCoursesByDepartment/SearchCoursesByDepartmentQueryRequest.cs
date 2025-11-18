using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Course.SearchCoursesByDepartment
{
    public class SearchCoursesByDepartmentQueryRequest : IRequest<SearchCoursesByDepartmentQueryResponse>
    {
        public Guid DepartmentId { get; set; }
        public string SearchTerm { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
