using MediatR;
using NotePool.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using D = NotePool.Domain.Entities;
using NotePool.Domain.Entities.Enums;

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
                    .ThenInclude(c => c.Notes)
                        .ThenInclude(n => n.Reactions)
                .Include(d => d.Courses)
                    .ThenInclude(c => c.Notes)
                        .ThenInclude(n => n.Comments)
                .Include(d => d.Courses)
                    .ThenInclude(c => c.Notes)
                        .ThenInclude(n => n.NoteDownloads)
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
                        course.Year,

                        NoteCount = course.Notes.Count(),
                        TotalLikes = course.Notes.SelectMany(n => n.Reactions).Count(r => r.Type == ReactionType.Like),
                        TotalComments = course.Notes.SelectMany(n => n.Comments).Count(),
                        TotalDownloads = course.Notes.SelectMany(n => n.NoteDownloads).Count() 
                    })
                    .OrderBy(c => c.Name)
                    .ToList()
            };
        }
    }
}
