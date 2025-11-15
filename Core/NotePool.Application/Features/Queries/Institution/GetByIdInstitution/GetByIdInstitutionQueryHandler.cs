using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using I = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Queries.Institution.GetByIdInstitution
{
    public class GetByIdInstitutionQueryHandler : IRequestHandler<GetByIdInstitutionQueryRequest, GetByIdInstitutionQueryResponse>
    {
        readonly IInstitutionReadRepository _institutionReadRepository;

        public GetByIdInstitutionQueryHandler(IInstitutionReadRepository institutionReadRepository)
        {
            _institutionReadRepository = institutionReadRepository;
        }

        public async Task<GetByIdInstitutionQueryResponse> Handle(GetByIdInstitutionQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _institutionReadRepository.GetAll(false);
            I.Institution institution = await query
                .Include(i => i.Departments) 
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken); 
            if (institution == null)
            {
                return null; 
            }

            return new GetByIdInstitutionQueryResponse
            {
                Name = institution.Name,
                City = institution.City,

                Departments = institution.Departments
                    .Select(dept => new
                    {
                        dept.Id,
                        dept.Name,
                        dept.Code
                    })
                    .OrderBy(d => d.Name) 
                    .ToList()
            };
        }
    }
}
