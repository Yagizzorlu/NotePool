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

namespace NotePool.Application.Features.Queries.Note.GetMyNotes
{
    public class GetMyNotesQueryHandler : IRequestHandler<GetMyNotesQueryRequest, GetMyNotesQueryResponse>
    {
        private readonly INoteReadRepository _noteReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyNotesQueryHandler(
            INoteReadRepository noteReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _noteReadRepository = noteReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetMyNotesQueryResponse> Handle(GetMyNotesQueryRequest request, CancellationToken cancellationToken)
        {
            var loggedInUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdString))
            {
                throw new UnauthorizedAccessException("Oturum açmanız gerekiyor.");
            }

            var loggedInUserId = Guid.Parse(loggedInUserIdString);

            var query = _noteReadRepository.GetAll(false)
                .Where(n => n.UserId == loggedInUserId);

            var totalCount = await query.CountAsync(cancellationToken);

            var notesList = await query
                .Include(n => n.Reactions)
                .Include(n => n.Comments)
                .Include(n => n.Bookmarks)
                .Include(n => n.NoteDownloads)
                .OrderByDescending(n => n.CreatedDate)
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .Select(note => new
                {
                    note.Id,
                    note.Title,
                    note.CreatedDate,
                    note.ViewCount,
                    note.DownloadCount,

                    LikeCount = note.Reactions.Count(r => r.Type == ReactionType.Like),
                    BookmarkCount = note.Bookmarks.Count,
                    CommentCount = note.Comments.Count(c => c.ParentId == null),
                    ReplyCount = note.Comments.Count(c => c.ParentId != null),

                    IsDownloaded = note.NoteDownloads.Any(d => d.UserId == loggedInUserId), 
                    note.UserId
                })
                .ToListAsync(cancellationToken);

            return new GetMyNotesQueryResponse
            {
                Notes = notesList,
                TotalCount = totalCount
            };
        }
    }
}
