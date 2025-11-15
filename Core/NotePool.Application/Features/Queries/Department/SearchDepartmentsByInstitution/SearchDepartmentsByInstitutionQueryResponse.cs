using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Department.SearchDepartmentsByInstitution
{
    public class SearchDepartmentsByInstitutionQueryResponse
    {
        public int TotalCount { get; set; }
        public object Departments { get; set; }
    }
}
