using MediatR;
using NotePool.Application.Features.Queries.Department.GetAllDepartment;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NotePool.Application.Features.Queries.Department.GetAllDepartment
{
    public class GetAllDepartmentQueryHandler : IRequestHandler<GetAllDepartmentQueryRequest, GetAllDepartmentQueryResponse>
    {
        readonly IDepartmentReadRepository _departmentReadRepository;

        public GetAllDepartmentQueryHandler(IDepartmentReadRepository departmentReadRepository)
        {
            _departmentReadRepository = departmentReadRepository;
        }

        public async Task<GetAllDepartmentQueryResponse> Handle(GetAllDepartmentQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _departmentReadRepository.GetAll(false);
            var totalCount = await query.CountAsync(cancellationToken);

            var departmentList = await query
                .OrderBy(d => d.Name)
                .Select(department => new
                {
                    department.Id,
                    department.Name,
                    department.Code,
                    department.InstitutionId
                })
                .ToListAsync(cancellationToken);
            return new()
            {
                Departments = departmentList,
                TotalCount = totalCount
            };
        }
    }
}
