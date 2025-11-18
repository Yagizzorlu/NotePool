using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NotePool.Application.Features.Queries.Note.GetAllNote;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities.Enums;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Note.GetAllNote
{
    public class GetAllNoteQueryHandler : IRequestHandler<GetAllNoteQueryRequest, GetAllNoteQueryResponse>
    {
        readonly INoteReadRepository _noteReadRepository;
        readonly ILogger<GetAllNoteQueryHandler> _logger;
        readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllNoteQueryHandler(
            INoteReadRepository noteReadRepository,
            ILogger<GetAllNoteQueryHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _noteReadRepository = noteReadRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetAllNoteQueryResponse> Handle(GetAllNoteQueryRequest request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? currentUserId = null;
            if (!string.IsNullOrEmpty(userIdString))
            {
                currentUserId = Guid.Parse(userIdString);
            }

            _logger.LogInformation("Get All Notes");
            var query = _noteReadRepository.GetAll(false);

            var totalCount = await query.CountAsync(cancellationToken);

            var notes = await query
                .Include(n => n.User)
                .Include(n => n.Comments)
                .Include(n => n.Reactions)
                .Include(n => n.Bookmarks)
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

                    LikeCount = n.Reactions.Count(r => r.Type == ReactionType.Like),
                    CommentCount = n.Comments.Count(c => c.ParentId == null),
                    ReplyCount = n.Comments.Count(c => c.ParentId != null),

                    IsBookmarked = currentUserId.HasValue && n.Bookmarks.Any(b => b.UserId == currentUserId.Value),

                    IsDownloaded = currentUserId.HasValue && n.NoteDownloads.Any(d => d.UserId == currentUserId.Value),

                    n.UserId,
                    AuthorName = n.User != null ? n.User.UserName : "Bilinmeyen Kullanıcı"
                })
                .ToListAsync(cancellationToken);

            return new()
            {
                Notes = notes,
                TotalCount = totalCount
            };
        }
    }
}