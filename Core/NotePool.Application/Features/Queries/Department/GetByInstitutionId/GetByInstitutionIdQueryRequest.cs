using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Department.GetByInstitutionId
{
    public class GetByInstitutionIdQueryRequest : IRequest<GetByInstitutionIdQueryResponse>
    {
        public Guid InstitutionId { get; set; }
    }
}
