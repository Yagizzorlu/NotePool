using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.User.GetUserByDepartmentId
{
    public class GetUserByDepartmentIdQueryRequest : IRequest<GetUserByDepartmentIdQueryResponse>
    {
        public Guid DepartmentId { get; set; }
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}
