using MediatR;
using NotePool.Application.Features.Queries.Department.GetByIdDepartment;
using NotePool.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C = NotePool.Domain.Entities;
using NotePool.Domain.Entities;

using NotePool.Application.Features.Queries.Course.GetByIdCourse;


namespace NotePool.Application.Features.Queries.Course.GetByIdCourse
{
    public class GetByIdCourseQueryHandler : IRequestHandler<GetByIdCourseQueryRequest, GetByIdCourseQueryResponse>
    {
        readonly ICourseReadRepository _courseReadRepository;

        public GetByIdCourseQueryHandler(ICourseReadRepository courseReadRepository)
        {
            _courseReadRepository = courseReadRepository;
        }

        public async Task<GetByIdCourseQueryResponse> Handle(GetByIdCourseQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _courseReadRepository.GetAll(false);

            C.Course course = await query
                .Include(c => c.Notes)
                    .ThenInclude(note => note.User)
                .Include(c => c.Notes)
                    .ThenInclude(note => note.Reactions)
                .Include(c => c.Notes)
                    .ThenInclude(note => note.Comments)
                .FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

            if (course == null)
            {
                return null;
            }

            return new GetByIdCourseQueryResponse
            {
                Name = course.Name,
                Code = course.Code,
                Year = course.Year,
                DepartmentId = course.DepartmentId,

                Notes = course.Notes
                    .Select(note => new
                    {
                        note.Id,
                        note.Title,
                        note.CreatedDate,
                        note.DownloadCount,
                        note.ViewCount,
                        //LikeCount = note.Reactions.Count(r => r.IsLike),
                        CommentCount = note.Comments.Count(),
                        note.UserId,
                        AuthorName = note.User.UserName
                    })
                    .OrderByDescending(n => n.CommentCount)
                    .ToList()
            };
        }
    }
}

