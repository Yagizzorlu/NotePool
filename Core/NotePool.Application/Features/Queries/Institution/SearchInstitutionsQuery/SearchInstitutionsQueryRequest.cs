using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Institution.SearchInstitutionsQuery
{
    public class SearchInstitutionsQueryRequest : IRequest<SearchInstitutionsQueryResponse>
    {
        public string SearchTerm { get; set; }
        public int PageSize { get; set; }
    }
}
