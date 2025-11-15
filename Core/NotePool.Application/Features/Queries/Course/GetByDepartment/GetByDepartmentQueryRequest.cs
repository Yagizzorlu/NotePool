using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Course.GetByDepartment
{
    public class GetByDepartmentQueryRequest : IRequest<GetByDepartmentQueryResponse>
    {
        public Guid DepartmentId { get; set; }
    }
}
