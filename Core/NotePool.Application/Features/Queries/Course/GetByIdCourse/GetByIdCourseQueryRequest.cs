using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Course.GetByIdCourse
{
    public class GetByIdCourseQueryRequest : IRequest<GetByIdCourseQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
