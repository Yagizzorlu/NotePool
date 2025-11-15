using MediatR;
using NotePool.Application.Features.Queries.Department.GetByInstitutionId;
using NotePool.Application.Repositories;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Course.GetByDepartment
{
    public class GetByDepartmentQueryHandler : IRequestHandler<GetByDepartmentQueryRequest, GetByDepartmentQueryResponse>
    {
        private readonly ICourseReadRepository _courseReadRepository;

        public GetByDepartmentQueryHandler(ICourseReadRepository courseReadRepository)
        {
            _courseReadRepository = courseReadRepository;
        }

        public async Task<GetByDepartmentQueryResponse> Handle(GetByDepartmentQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _courseReadRepository.GetAll(false);
            var filteredQuery = query.Where(c => c.DepartmentId == request.DepartmentId);

            var totalCount = await filteredQuery.CountAsync(cancellationToken);

            var courseList = await filteredQuery
                .OrderBy(c => c.Name)
                .Select(course => new
                {

                    course.Id,
                    course.Name,
                    course.Year,
                    course.Code
                })
                .ToListAsync(cancellationToken);

            return new GetByDepartmentQueryResponse
            {
                Courses = courseList,
                TotalCount = totalCount
            };
        }
    }
}
