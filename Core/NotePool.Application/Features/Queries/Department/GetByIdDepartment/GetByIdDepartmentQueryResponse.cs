using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Department.GetByIdDepartment
{
    public class GetByIdDepartmentQueryResponse
    {
        public string Name { get; set; }
        public string? Code { get; set; }
        public Guid InstitutionId { get; set; }
        public object Courses { get; set; }
    }
}
