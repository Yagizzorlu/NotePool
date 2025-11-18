using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NotePool.Application.Features.Queries.Note.GetByCourseId;

namespace NotePool.Application.Features.Queries.Note.GetByCourseId
{
    public class GetByCourseIdQueryHandler : IRequestHandler<GetByCourseIdQueryRequest, GetByCourseIdQueryResponse>
    {
        private readonly INoteReadRepository _noteReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetByCourseIdQueryHandler(
            INoteReadRepository noteReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _noteReadRepository = noteReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetByCourseIdQueryResponse> Handle(GetByCourseIdQueryRequest request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? currentUserId = null;

            if (!string.IsNullOrEmpty(userIdString))
            {
                currentUserId = Guid.Parse(userIdString);
            }

            var query = _noteReadRepository.GetAll(false);

            var filteredQuery = query.Where(n => n.CourseId == request.CourseId);

            var totalCount = await filteredQuery.CountAsync(cancellationToken);

            var noteList = await filteredQuery
                .Include(n => n.User)
                .Include(n => n.Reactions)
                .Include(n => n.Bookmarks)
                .Include(n => n.Comments)
                .Include(n => n.NoteDownloads)
                .OrderByDescending(n => n.CreatedDate)
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .Select(n => new
                {
                    n.Id,
                    n.Title,
                    DescriptionSnippet = n.Description != null && n.Description.Length > 150
                                            ? n.Description.Substring(0, 150) + "..."
                                            : n.Description ?? string.Empty,
                    n.CreatedDate,

                    n.ViewCount,
                    n.DownloadCount,

                    IsBookmarked = currentUserId.HasValue &&
                                   n.Bookmarks.Any(b => b.UserId == currentUserId.Value),
                    IsDownloaded = currentUserId.HasValue && 
                                   n.NoteDownloads.Any(d => d.UserId == currentUserId.Value),

                    BookmarkCount = n.Bookmarks.Count,

                    LikeCount = n.Reactions.Count(r => r.Type == ReactionType.Like),
                    CommentCount = n.Comments.Count(c => c.ParentId == null),
                    ReplyCount = n.Comments.Count(c => c.ParentId != null),

                    n.UserId,
                    AuthorName = n.User != null ? n.User.UserName : "Bilinmeyen Kullanıcı"
                })
                .ToListAsync(cancellationToken);

            return new GetByCourseIdQueryResponse
            {
                Notes = noteList,
                TotalCount = totalCount
            };
        }
    }
}