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

namespace NotePool.Application.Features.Queries.Reaction.GetReactionsByUserId
{
    public class GetReactionsByUserIdQueryHandler : IRequestHandler<GetReactionsByUserIdQueryRequest, GetReactionsByUserIdQueryResponse>
    {
        private readonly IReactionReadRepository _reactionReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetReactionsByUserIdQueryHandler(
            IReactionReadRepository reactionReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _reactionReadRepository = reactionReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetReactionsByUserIdQueryResponse> Handle(GetReactionsByUserIdQueryRequest request, CancellationToken cancellationToken)
        {
            var currentUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? currentUserId = null;
            if (!string.IsNullOrEmpty(currentUserIdString))
            {
                currentUserId = Guid.Parse(currentUserIdString);
            }

            var requestedUserId = Guid.Parse(request.Id);

            var query = _reactionReadRepository.GetAll(false)
                .Where(r => r.UserId == requestedUserId && r.Type == request.Type);

            var totalCount = await query.CountAsync(cancellationToken);

            var noteList = await query
                .Include(r => r.Note)
                    .ThenInclude(note => note.User)
                .Include(r => r.Note)
                    .ThenInclude(note => note.Comments)
                .Include(r => r.Note)
                    .ThenInclude(note => note.Reactions)
                .Include(r => r.Note)
                    .ThenInclude(note => note.Bookmarks) 
                .Include(r => r.Note)
                    .ThenInclude(note => note.NoteDownloads) 
                .OrderByDescending(r => r.CreatedDate)
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .Select(r => r.Note)
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

            return new GetReactionsByUserIdQueryResponse
            {
                Notes = noteList,
                TotalCount = totalCount
            };
        }
    }
}
