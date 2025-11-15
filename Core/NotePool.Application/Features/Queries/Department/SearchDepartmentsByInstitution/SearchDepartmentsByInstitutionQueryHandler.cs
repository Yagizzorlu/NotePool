using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NotePool.Application.Features.Queries.Department.SearchDepartmentsByInstitution;

namespace NotePool.Application.Features.Queries.Department.SearchByInstitutionId
{
    public class SearchDepartmentsByInstitutionQueryHandler : IRequestHandler<SearchDepartmentsByInstitutionQueryRequest, SearchDepartmentsByInstitutionQueryResponse>
    {
        private readonly IDepartmentReadRepository _departmentReadRepository;

        public SearchDepartmentsByInstitutionQueryHandler(IDepartmentReadRepository departmentReadRepository)
        {
            _departmentReadRepository = departmentReadRepository;
        }
        public async Task<SearchDepartmentsByInstitutionQueryResponse> Handle(SearchDepartmentsByInstitutionQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _departmentReadRepository.GetAll(false);

            var filteredByInstQuery = query.Where(d => d.InstitutionId == request.InstitutionId);


            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                filteredByInstQuery = filteredByInstQuery
                    .Where(d =>
                        d.Name.Contains(request.SearchTerm) ||
                        (d.Code != null && d.Code.Contains(request.SearchTerm))
                    );
            }

            var totalCount = await filteredByInstQuery.CountAsync(cancellationToken);

            var departmentList = await filteredByInstQuery
                .OrderBy(d => d.Name)
                .Take(request.PageSize)
                .Select(department => new
                {
                    department.Id,
                    department.Name,
                    department.Code
                })
                .ToListAsync(cancellationToken);

            return new SearchDepartmentsByInstitutionQueryResponse
            {
                Departments = departmentList,
                TotalCount = totalCount
            };
        }
    }
}
