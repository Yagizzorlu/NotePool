using MediatR;
using NotePool.Application.Features.Queries.Department.GetByIdDepartment;
using NotePool.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D = NotePool.Domain.Entities;
using NotePool.Domain.Entities.Enums;

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
                .Select(department => new
                {
                    department.Id,
                    department.Name,
                    department.Code,

                    CourseCount = department.Courses.Count(),
                    TotalNoteCount = department.Courses.SelectMany(c => c.Notes).Count(),
                    TotalLikeCount = department.Courses.SelectMany(c => c.Notes).SelectMany(n => n.Reactions).Count(r => r.Type == ReactionType.Like),

                    TotalComments = department.Courses.SelectMany(c => c.Notes).SelectMany(n => n.Comments).Count(),
                    TotalDownloads = department.Courses.SelectMany(c => c.Notes).SelectMany(n => n.NoteDownloads).Count()
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
