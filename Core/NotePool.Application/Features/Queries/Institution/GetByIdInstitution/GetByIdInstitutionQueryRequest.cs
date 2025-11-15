using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Institution.GetByIdInstitution
{
    public class GetByIdInstitutionQueryRequest : IRequest<GetByIdInstitutionQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
