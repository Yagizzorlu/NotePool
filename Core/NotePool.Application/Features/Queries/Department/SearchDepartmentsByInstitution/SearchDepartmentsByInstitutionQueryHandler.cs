using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Department.SearchDepartmentsByInstitution
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
                        d.Code != null && d.Code.Contains(request.SearchTerm)
                    );
            }

            var totalCount = await filteredByInstQuery.CountAsync(cancellationToken);

            var departmentList = await filteredByInstQuery
                .Include(d => d.Courses)
                    .ThenInclude(c => c.Notes)
                        .ThenInclude(n => n.Reactions) 
                .Include(d => d.Courses)
                    .ThenInclude(c => c.Notes)
                        .ThenInclude(n => n.Comments) 
                .Include(d => d.Courses)
                    .ThenInclude(c => c.Notes)
                        .ThenInclude(n => n.NoteDownloads) 
                                                           
                .OrderBy(d => d.Name)
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize)
                .Select(department => new
                {
                    department.Id,
                    department.Name,
                    department.Code,

                    TotalCourseCount = department.Courses.Count(),
                    TotalNoteCount = department.Courses.SelectMany(c => c.Notes).Count(),
                    TotalComments = department.Courses.SelectMany(c => c.Notes).SelectMany(n => n.Comments).Count(),
                    TotalDownloads = department.Courses.SelectMany(c => c.Notes).SelectMany(n => n.NoteDownloads).Count(),
                    TotalReactions = department.Courses.SelectMany(c => c.Notes).SelectMany(n => n.Reactions).Count()
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
