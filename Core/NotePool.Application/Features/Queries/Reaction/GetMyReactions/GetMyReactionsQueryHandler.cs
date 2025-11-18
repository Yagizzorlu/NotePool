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
using NotePool.Domain.Entities;

namespace NotePool.Application.Features.Queries.Reaction.GetMyReactions
{
    public class GetMyReactionsQueryHandler : IRequestHandler<GetMyReactionsQueryRequest, GetMyReactionsQueryResponse>
    {
        private readonly IReactionReadRepository _reactionReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyReactionsQueryHandler(
            IReactionReadRepository reactionReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _reactionReadRepository = reactionReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetMyReactionsQueryResponse> Handle(GetMyReactionsQueryRequest request, CancellationToken cancellationToken)
        {
            var loggedInUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdString))
                throw new UnauthorizedAccessException("Kullanıcı girişi bulunamadı.");

            var loggedInUserId = Guid.Parse(loggedInUserIdString);

            var query = _reactionReadRepository.GetAll(false)
                .Where(r => r.UserId == loggedInUserId && r.Type == request.Type);

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

                    IsBookmarked = note.Bookmarks.Any(b => b.UserId == loggedInUserId),
                    IsDownloaded = note.NoteDownloads.Any(d => d.UserId == loggedInUserId), 

                    BookmarkCount = note.Bookmarks.Count,
                    LikeCount = note.Reactions.Count(r => r.Type == ReactionType.Like),
                    CommentCount = note.Comments.Count(c => c.ParentId == null),
                    ReplyCount = note.Comments.Count(c => c.ParentId != null),

                    note.UserId,
                    AuthorName = note.User != null ? note.User.UserName : "Bilinmeyen Kullanıcı"
                })
                .ToListAsync(cancellationToken);

            return new GetMyReactionsQueryResponse
            {
                Notes = noteList,
                TotalCount = totalCount
            };
        }
    }
}
