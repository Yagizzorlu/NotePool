using MediatR;
using NotePool.Application.Features.Queries.Department.GetByIdDepartment;
using NotePool.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Department.GetByInstitutionId
{
    public class GetByInstitutionIdQueryHandler : IRequestHandler<GetByInstitutionIdQueryRequest, GetByInstitutionIdQueryResponse>
    {
        private readonly IDepartmentReadRepository _departmentReadRepository;

        public GetByInstitutionIdQueryHandler(IDepartmentReadRepository departmentReadRepository)
        {
            _departmentReadRepository = departmentReadRepository;
        }

        public async Task<GetByInstitutionIdQueryResponse> Handle(GetByInstitutionIdQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _departmentReadRepository.GetAll(false);


            var filteredQuery = query.Where(d => d.InstitutionId == request.InstitutionId);

            var totalCount = await filteredQuery.CountAsync(cancellationToken);

            var departmentList = await filteredQuery
                .OrderBy(d => d.Name)
                .Select(department => new
                {

                    department.Id,
                    department.Name,
                    department.Code
                })
                .ToListAsync(cancellationToken);

            return new GetByInstitutionIdQueryResponse
            {
                Departments = departmentList,
                TotalCount = totalCount
            };
        }
    }
}
