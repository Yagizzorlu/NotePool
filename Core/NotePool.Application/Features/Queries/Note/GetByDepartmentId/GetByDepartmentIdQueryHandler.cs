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
using NotePool.Domain.Entities;

namespace NotePool.Application.Features.Queries.Note.GetByDepartmentId
{
    public class GetByDepartmentIdQueryHandler : IRequestHandler<GetByDepartmentIdQueryRequest, GetByDepartmentIdQueryResponse>
    {
        private readonly INoteReadRepository _noteReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetByDepartmentIdQueryHandler(INoteReadRepository noteReadRepository, IHttpContextAccessor httpContextAccessor)
        {
            _noteReadRepository = noteReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetByDepartmentIdQueryResponse> Handle(GetByDepartmentIdQueryRequest request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? currentUserId = null;

            if (!string.IsNullOrEmpty(userIdString))
            {
                currentUserId = Guid.Parse(userIdString);
            }

            var query = _noteReadRepository.GetAll(false);

            var filteredQuery = query.Where(n => n.DepartmentId == request.DepartmentId);

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

            return new GetByDepartmentIdQueryResponse
            {
                Notes = noteList,
                TotalCount = totalCount
            };
        }
    }
}
