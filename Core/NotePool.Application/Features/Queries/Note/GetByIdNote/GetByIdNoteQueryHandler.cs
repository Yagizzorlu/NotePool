using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities;
using NotePool.Application.Features.Queries.Note.GetByIdNote;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using N = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Queries.Note.GetByIdNote
{
    public class GetByIdNoteQueryHandler : IRequestHandler<GetByIdNoteQueryRequest, GetByIdNoteQueryResponse>
    {
        readonly INoteReadRepository _noteReadRepository;

        public GetByIdNoteQueryHandler(INoteReadRepository noteReadRepository)
        {
            _noteReadRepository = noteReadRepository;
        }

        public async Task<GetByIdNoteQueryResponse> Handle(GetByIdNoteQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _noteReadRepository.GetAll(false);

            var note = await query
                .Include(n => n.User)
                .Include(n => n.NotePdfFiles)
                .Include(n => n.Reactions)
                .Include(n => n.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(n => n.Id == Guid.Parse(request.Id), cancellationToken);

            if (note == null)
            {
                return null;
            }

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
                // LikeCount = note.Reactions.Count(r => r.IsLike), // (Aklımda)

                AuthorId = note.UserId,
                AuthorName = note.User.UserName,

                PdfFiles = note.NotePdfFiles.Select(file => new
                {
                    file.Id,
                    file.FileName,
                    file.Path
                }).ToList(),

                Comments = note.Comments
                    .Select(comment => new
                    {
                        comment.Id,
                        comment.Content,
                        comment.CreatedDate,
                        CommentAuthorId = comment.UserId,
                        CommentAuthorName = comment.User.UserName
                    })
                    .OrderBy(c => c.CreatedDate)
                    .ToList()
            };
        }
    }
}
