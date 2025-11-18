using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Download.GetNoteDownloadsByUserId
{
    public class GetNoteDownloadsByUserIdQueryHandler : IRequestHandler<GetNoteDownloadsByUserIdQueryRequest, GetNoteDownloadsByUserIdQueryResponse>
    {
        private readonly INoteDownloadReadRepository _noteDownloadReadRepository;

        public GetNoteDownloadsByUserIdQueryHandler(INoteDownloadReadRepository noteDownloadReadRepository)
        {
            _noteDownloadReadRepository = noteDownloadReadRepository;
        }

        public async Task<GetNoteDownloadsByUserIdQueryResponse> Handle(GetNoteDownloadsByUserIdQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _noteDownloadReadRepository.GetAll(false)
                .Where(nd => nd.UserId == request.UserId);

            var totalCount = await query.CountAsync(cancellationToken);

            var downloadsList = await query
                .Include(nd => nd.Note)
                    .ThenInclude(n => n.User)
                .Include(nd => nd.Note).ThenInclude(n => n.Reactions)
                .Include(nd => nd.Note).ThenInclude(n => n.Comments)
                .Include(nd => nd.Note).ThenInclude(n => n.Bookmarks)

                .OrderByDescending(nd => nd.CreatedDate)
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .Select(nd => new
                {
                    DownloadDate = nd.CreatedDate,

                    NoteId = nd.Note != null ? nd.Note.Id : Guid.Empty,
                    NoteTitle = nd.Note != null ? nd.Note.Title : "Silinmiş Not",
                    NoteDescriptionSnippet = nd.Note != null && nd.Note.Description != null && nd.Note.Description.Length > 150
                                            ? nd.Note.Description.Substring(0, 150) + "..."
                                            : nd.Note != null ? (nd.Note.Description ?? string.Empty) : string.Empty,
                    AuthorId = nd.Note != null ? nd.Note.UserId : Guid.Empty,
                    AuthorName = nd.Note != null && nd.Note.User != null ? nd.Note.User.UserName : "Bilinmeyen Kullanıcı",
                    NoteViewCount = nd.Note != null ? nd.Note.ViewCount : 0,
                    NoteDownloadCount = nd.Note != null ? nd.Note.DownloadCount : 0,
                    NoteLikeCount = nd.Note != null ? nd.Note.Reactions.Count(r => r.Type == ReactionType.Like) : 0,
                    NoteCommentCount = nd.Note != null ? nd.Note.Comments.Count() : 0,
                    NoteBookmarkCount = nd.Note != null ? nd.Note.Bookmarks.Count() : 0
                })
                .ToListAsync(cancellationToken);

            return new GetNoteDownloadsByUserIdQueryResponse
            {
                Downloads = downloadsList,
                TotalCount = totalCount
            };
        }
    }
}
