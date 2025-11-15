using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Institution.GetByIdInstitution
{
    public class GetByIdInstitutionQueryResponse
    {
        public string Name { get; set; }
        public string? City { get; set; }
        public object Departments { get; set; }
    }
}
