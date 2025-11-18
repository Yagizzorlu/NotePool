using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories; 
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Courses.Queries.SearchByDepartment;
using NotePool.Application.Features.Queries.Course.SearchCoursesByDepartment;


namespace Application.Features.Courses.Queries.SearchByDepartment
{
    public class SearchCoursesByDepartmentQueryHandler : IRequestHandler<SearchCoursesByDepartmentQueryRequest, SearchCoursesByDepartmentQueryResponse>
    {
        private readonly ICourseReadRepository _courseReadRepository;

        public SearchCoursesByDepartmentQueryHandler(ICourseReadRepository courseReadRepository)
        {
            _courseReadRepository = courseReadRepository;
        }

        public async Task<SearchCoursesByDepartmentQueryResponse> Handle(SearchCoursesByDepartmentQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _courseReadRepository.GetAll(false); 

            var filteredByDeptQuery = query.Where(c => c.DepartmentId == request.DepartmentId);


            if (!string.IsNullOrEmpty(request.SearchTerm))
            {

                filteredByDeptQuery = filteredByDeptQuery
                    .Where(c =>
                        c.Name.Contains(request.SearchTerm) ||
                        c.Code.Contains(request.SearchTerm)
                    );
            }
            var totalCount = await filteredByDeptQuery.CountAsync(cancellationToken);
            var courseList = await filteredByDeptQuery
                .OrderBy(c => c.Name)
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize) 
                .Select(course => new
                {
                    course.Id,
                    course.Name,
                    course.Code,
                    course.Year
                })
                .ToListAsync(cancellationToken);
            return new SearchCoursesByDepartmentQueryResponse
            {
                Courses = courseList,
                TotalCount = totalCount
            };
        }
    }
}
