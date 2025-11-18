using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.User.GetUserByDepartmentId
{
    public class GetUserByDepartmentIdQueryResponse
    {
        public object Users { get; set; }
        public int TotalCount { get; set; }
    }
}
