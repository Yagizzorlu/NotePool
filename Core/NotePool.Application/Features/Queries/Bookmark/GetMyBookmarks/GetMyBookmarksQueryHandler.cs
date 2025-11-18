using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities.Enums;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Bookmark.GetMyBookmarks
{
    public class GetMyBookmarksQueryHandler : IRequestHandler<GetMyBookmarksQueryRequest, GetMyBookmarksQueryResponse>
    {
        private readonly IBookmarkReadRepository _bookmarkReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyBookmarksQueryHandler(
            IBookmarkReadRepository bookmarkReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _bookmarkReadRepository = bookmarkReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetMyBookmarksQueryResponse> Handle(GetMyBookmarksQueryRequest request, CancellationToken cancellationToken)
        {
            var loggedInUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdString))
                throw new UnauthorizedAccessException("Kullanıcı girişi bulunamadı.");

            var loggedInUserId = Guid.Parse(loggedInUserIdString);

            var query = _bookmarkReadRepository.GetAll(false)
                .Where(b => b.UserId == loggedInUserId);

            var totalCount = await query.CountAsync(cancellationToken);

            var noteList = await query
                .Include(b => b.Note)
                    .ThenInclude(note => note.User)
                .Include(b => b.Note)
                    .ThenInclude(note => note.Comments)
                .Include(b => b.Note)
                    .ThenInclude(note => note.Reactions)
                .Include(b => b.Note)
                    .ThenInclude(note => note.NoteDownloads) 
                .OrderByDescending(b => b.CreatedDate)
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .Select(b => b.Note)
                .Select(note => new
                {
                    note.Id,
                    note.Title,
                    DescriptionSnippet = note.Description != null && note.Description.Length > 150
                                            ? note.Description.Substring(0, 150) + "..."
                                            : note.Description ?? string.Empty,
                    note.CreatedDate,
                    note.ViewCount,
                    note.DownloadCount,
                    IsBookmarked = true, 
                    IsDownloaded = note.NoteDownloads.Any(d => d.UserId == loggedInUserId), 
                    LikeCount = note.Reactions.Count(r => r.Type == ReactionType.Like),
                    CommentCount = note.Comments.Count(c => c.ParentId == null),
                    ReplyCount = note.Comments.Count(c => c.ParentId != null),
                    BookmarkCount = note.Bookmarks.Count,

                    note.UserId,
                    AuthorName = note.User != null ? note.User.UserName : "Bilinmeyen Kullanıcı",
                })
                .ToListAsync(cancellationToken);

            return new GetMyBookmarksQueryResponse
            {
                Notes = noteList,
                TotalCount = totalCount
            };
        }
    }
}
