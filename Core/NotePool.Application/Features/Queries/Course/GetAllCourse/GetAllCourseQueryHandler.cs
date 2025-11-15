using MediatR;
using NotePool.Application.Features.Queries.Course.GetAllCourse;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Course.GetAllCourse
{
    public class GetAllCourseQueryHandler : IRequestHandler<GetAllCourseQueryRequest,GetAllCourseQueryResponse>
    {
        readonly ICourseReadRepository _courseReadRepository;

        public GetAllCourseQueryHandler(ICourseReadRepository courseReadRepository)
        {
            _courseReadRepository = courseReadRepository;
        }

        public async Task<GetAllCourseQueryResponse> Handle(GetAllCourseQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _courseReadRepository.GetAll(false);
            var totalCount = await query.CountAsync(cancellationToken);

            var courseList = await query
                .OrderBy(c => c.Name)
                .Select(course => new
                {
                    course.Id,
                    course.Name,
                    course.Code,
                    course.Year,
                    course.DepartmentId
                })
                .ToListAsync(cancellationToken);
            return new()
            {
                Courses = courseList,
                TotalCount = totalCount
            };
        }
    }
}

