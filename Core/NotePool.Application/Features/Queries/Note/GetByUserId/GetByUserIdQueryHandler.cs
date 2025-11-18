using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Features.Queries.Note.GetByUserId;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities.Enums;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Note.GetNotesByUserId
{
    public class GetByUserIdQueryHandler : IRequestHandler<GetByUserIdQueryRequest, GetByUserIdQueryResponse>
    {
        private readonly INoteReadRepository _noteReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetByUserIdQueryHandler(INoteReadRepository noteReadRepository, IHttpContextAccessor httpContextAccessor)
        {
            _noteReadRepository = noteReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetByUserIdQueryResponse> Handle(GetByUserIdQueryRequest request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? currentUserId = null;

            if (!string.IsNullOrEmpty(userIdString))
            {
                currentUserId = Guid.Parse(userIdString);
            }

            var query = _noteReadRepository.GetAll(false);

            var filteredQuery = query.Where(n => n.UserId == Guid.Parse(request.Id));

            var totalCount = await filteredQuery.CountAsync(cancellationToken);

            var noteList = await filteredQuery
                .Include(n => n.User)
                .Include(n => n.Comments)
                .Include(n => n.Reactions)
                .Include(n => n.Bookmarks)
                .Include(n => n.NoteDownloads)
                .OrderByDescending(n => n.CreatedDate)
                .Skip(request.Page * request.Size)
                .Take(request.Size)
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

                    IsBookmarked = currentUserId.HasValue &&
                                   note.Bookmarks.Any(b => b.UserId == currentUserId.Value),
                    IsDownloaded = currentUserId.HasValue && 
                                   note.NoteDownloads.Any(d => d.UserId == currentUserId.Value),

                    BookmarkCount = note.Bookmarks.Count,

                    LikeCount = note.Reactions.Count(r => r.Type == ReactionType.Like),
                    CommentCount = note.Comments.Count(c => c.ParentId == null),
                    ReplyCount = note.Comments.Count(c => c.ParentId != null),
                    note.UserId,
                    AuthorName = note.User != null ? note.User.UserName : "Bilinmeyen Kullanıcı"
                })
                .ToListAsync(cancellationToken);

            return new GetByUserIdQueryResponse
            {
                Notes = noteList,
                TotalCount = totalCount
            };
        }
    }
}