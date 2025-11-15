using MediatR;
using NotePool.Application.Features.Queries.Department.GetByIdDepartment;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Queries.Department.GetByIdDepartment
{
    public class GetByIdDepartmentQueryHandler : IRequestHandler<GetByIdDepartmentQueryRequest, GetByIdDepartmentQueryResponse>
    {

        readonly IDepartmentReadRepository _departmentReadRepository;

        public GetByIdDepartmentQueryHandler(IDepartmentReadRepository departmentReadRepository)
        {
            _departmentReadRepository = departmentReadRepository;
        }

        public async Task<GetByIdDepartmentQueryResponse> Handle(GetByIdDepartmentQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _departmentReadRepository.GetAll(false);
            D.Department department = await query
                .Include(d => d.Courses)
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);
            if (department == null)
            {
                return null;
            }

            return new GetByIdDepartmentQueryResponse
            {
                Name = department.Name,
                Code = department.Code,
                InstitutionId = department.InstitutionId,

                Courses = department.Courses
                    .Select(course => new
                    {
                        course.Id,
                        course.Name,
                        course.Code,
                        course.Year
                    })
                    .OrderBy(c => c.Name)
                    .ToList()
            };
        }

    }
}
