using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Features.Queries.Course.GetByIdCourse;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities.Enums;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using C = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Queries.Course.GetByIdCourse
{
    public class GetByIdCourseQueryHandler : IRequestHandler<GetByIdCourseQueryRequest, GetByIdCourseQueryResponse>
    {
        readonly ICourseReadRepository _courseReadRepository;
        readonly IHttpContextAccessor _httpContextAccessor;

        public GetByIdCourseQueryHandler(
            ICourseReadRepository courseReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _courseReadRepository = courseReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetByIdCourseQueryResponse> Handle(GetByIdCourseQueryRequest request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? currentUserId = null;
            if (!string.IsNullOrEmpty(userIdString))
            {
                currentUserId = Guid.Parse(userIdString);
            }

            var query = _courseReadRepository.GetAll(false);

            C.Course course = await query
                .Include(c => c.Notes)
                    .ThenInclude(note => note.User)
                .Include(c => c.Notes)
                    .ThenInclude(note => note.Reactions)
                .Include(c => c.Notes)
                    .ThenInclude(note => note.Comments)
                .Include(c => c.Notes)
                    .ThenInclude(note => note.Bookmarks)
                .Include(c => c.Notes)
                    .ThenInclude(note => note.NoteDownloads)
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

                        IsBookmarked = currentUserId.HasValue && note.Bookmarks.Any(b => b.UserId == currentUserId.Value),
                        IsDownloaded = currentUserId.HasValue && note.NoteDownloads.Any(d => d.UserId == currentUserId.Value), 

                        LikeCount = note.Reactions.Count(r => r.Type == ReactionType.Like),
                        CommentCount = note.Comments.Count(c => c.ParentId == null),
                        ReplyCount = note.Comments.Count(c => c.ParentId != null),

                        note.UserId,
                        AuthorName = note.User?.UserName 
                    })
                    .OrderByDescending(n => n.CommentCount)
                    .ToList()
            };
        }
    }
}
