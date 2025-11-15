using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Institution.SearchInstitutionsQuery
{
    public class SearchInstitutionsQueryResponse
    {
        public int TotalCount { get; set; }
        public object Institutions { get; set; }
    }
}
