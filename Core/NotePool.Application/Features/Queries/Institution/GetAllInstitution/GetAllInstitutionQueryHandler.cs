using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Institution.GetAllInstitution
{
    public class GetAllInstitutionQueryHandler : IRequestHandler<GetAllInstitutionQueryRequest, GetAllInstitutionQueryResponse>
    {
        readonly IInstitutionReadRepository _institutionReadRepository;

        public GetAllInstitutionQueryHandler(IInstitutionReadRepository institutionReadRepository)
        {
            _institutionReadRepository = institutionReadRepository;
        }

        public async Task<GetAllInstitutionQueryResponse> Handle(GetAllInstitutionQueryRequest request, CancellationToken cancellationToken)
        {
                var query = _institutionReadRepository.GetAll(false);
                var totalCount = await query.CountAsync(cancellationToken); 

                var institutionList = await query
                    .OrderBy(i => i.Name) 
                    .Select(institution => new
                    {
                        institution.Id,
                        institution.Name,
                        institution.City
                    })
                    .ToListAsync(cancellationToken);
                 return new()
                 {
                     Institutions = institutionList,
                     TotalCount = totalCount
                 };
        }
    }
}
