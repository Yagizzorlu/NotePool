using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Features.Queries.Note.GetByIdNote;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities.Enums;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using N = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Queries.Note.GetByIdNote
{
    public class GetByIdNoteQueryHandler : IRequestHandler<GetByIdNoteQueryRequest, GetByIdNoteQueryResponse>
    {
        readonly INoteReadRepository _noteReadRepository;
        readonly INoteWriteRepository _noteWriteRepository;
        readonly IHttpContextAccessor _httpContextAccessor;

        public GetByIdNoteQueryHandler(
            INoteReadRepository noteReadRepository,
            INoteWriteRepository noteWriteRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _noteReadRepository = noteReadRepository;
            _noteWriteRepository = noteWriteRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetByIdNoteQueryResponse> Handle(GetByIdNoteQueryRequest request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? currentUserId = null;
            if (!string.IsNullOrEmpty(userIdString))
            {
                currentUserId = Guid.Parse(userIdString);
            }

            var query = _noteReadRepository.GetAll(false);

            var note = await query
                .Include(n => n.User)
                .Include(n => n.NotePdfFiles)
                .Include(n => n.Bookmarks)
                .Include(n => n.Reactions)
                    .ThenInclude(r => r.User)
                .Include(n => n.Comments)
                    .ThenInclude(c => c.User)
                .Include(n => n.Comments)
                    .ThenInclude(c => c.Replies)
                        .ThenInclude(r => r.User)
                .Include(n => n.NoteDownloads)
                .FirstOrDefaultAsync(n => n.Id == Guid.Parse(request.Id), cancellationToken);

            if (note == null)
            {
                return null;
            }

            note.ViewCount++;
            _noteWriteRepository.Update(note);
            await _noteWriteRepository.SaveAsync();

            return new GetByIdNoteQueryResponse
            {
                Id = note.Id,
                Title = note.Title,
                Description = note.Description,
                Tags = note.Tags,
                CreatedDate = note.CreatedDate,

                ViewCount = note.ViewCount,
                DownloadCount = note.DownloadCount,
                CommentCount = note.Comments.Count(),
                LikeCount = note.Reactions.Count(r => r.Type == ReactionType.Like),

                AuthorId = note.UserId,
                AuthorName = note.User != null ? note.User.UserName : "Bilinmeyen Kullanıcı",

                IsBookmarked = currentUserId.HasValue && note.Bookmarks.Any(b => b.UserId == currentUserId.Value),
                IsDownloaded = currentUserId.HasValue && note.NoteDownloads.Any(d => d.UserId == currentUserId.Value),

                Bookmarks = note.Bookmarks.Select(bookmark => new
                {
                    bookmark.Id,
                    BookmarkUserId = bookmark.UserId,
                    BookmarkUserName = bookmark.User?.UserName,
                }).ToList(),

                PdfFiles = note.NotePdfFiles.Select(file => new
                {
                    file.Id,
                    file.FileName,
                    file.Path
                }).ToList(),

                Comments = note.Comments
                    .Where(c => c.ParentId == null)
                    .Select(comment => new
                    {
                        comment.Id,
                        comment.Content,
                        comment.CreatedDate,
                        CommentAuthorId = comment.UserId,
                        CommentAuthorName = comment.User?.UserName,

                        Replies = comment.Replies.Select(reply => new
                        {
                            reply.Id,
                            reply.Content,
                            reply.CreatedDate,
                            ReplyAuthorId = reply.UserId,
                            ReplyAuthorName = reply.User?.UserName
                        })
                        .OrderBy(r => r.CreatedDate)
                        .ToList()
                    })
                    .OrderBy(c => c.CreatedDate)
                    .ToList(),

                Reactions = note.Reactions.Select(reaction => new
                {
                    reaction.Id,
                    reaction.Type,
                    ReactionAuthorId = reaction.UserId,
                    ReactionAuthorName = reaction.User?.UserName
                }).ToList()
            };
        }
    }
}
