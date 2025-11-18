using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Department.SearchDepartmentsByInstitution
{
    public class SearchDepartmentsByInstitutionQueryRequest : IRequest<SearchDepartmentsByInstitutionQueryResponse>
    {
        public Guid InstitutionId { get; set; }
        public string SearchTerm { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        }
}
