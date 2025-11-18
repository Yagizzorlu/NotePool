using MediatR;
using NotePool.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using I = NotePool.Domain.Entities;
using NotePool.Domain.Entities.Enums;

namespace NotePool.Application.Features.Queries.Institution.GetByIdInstitution
{
    public class GetByIdInstitutionQueryHandler : IRequestHandler<GetByIdInstitutionQueryRequest, GetByIdInstitutionQueryResponse>
    {
        readonly IInstitutionReadRepository _institutionReadRepository;

        public GetByIdInstitutionQueryHandler(IInstitutionReadRepository institutionReadRepository)
        {
            _institutionReadRepository = institutionReadRepository;
        }

        public async Task<GetByIdInstitutionQueryResponse> Handle(GetByIdInstitutionQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _institutionReadRepository.GetAll(false);

            I.Institution institution = await query
                .Include(i => i.Departments)
                    .ThenInclude(d => d.Courses)
                        .ThenInclude(c => c.Notes)
                            .ThenInclude(n => n.Reactions)
                .Include(i => i.Departments)
                    .ThenInclude(d => d.Courses)
                        .ThenInclude(c => c.Notes)
                            .ThenInclude(n => n.Comments)
                .Include(i => i.Departments)
                    .ThenInclude(d => d.Courses)
                        .ThenInclude(c => c.Notes)
                            .ThenInclude(n => n.NoteDownloads)
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (institution == null)
            {
                return null;
            }

            return new GetByIdInstitutionQueryResponse
            {
                Name = institution.Name,
                City = institution.City,

                Departments = institution.Departments
                    .Select(dept => new
                    {
                        dept.Id,
                        dept.Name,
                        dept.Code,

                        CourseCount = dept.Courses.Count(),
                        TotalNoteCount = dept.Courses.SelectMany(c => c.Notes).Count(),
                        TotalLikeCount = dept.Courses.SelectMany(c => c.Notes).SelectMany(n => n.Reactions).Count(r => r.Type == ReactionType.Like),
                        TotalCommentCount = dept.Courses.SelectMany(c => c.Notes).SelectMany(n => n.Comments).Count(),
                        TotalDownloadCount = dept.Courses.SelectMany(c => c.Notes).SelectMany(n => n.NoteDownloads).Count() 
                    })
                    .OrderBy(d => d.Name)
                    .ToList()
            };
        }
    }
}
