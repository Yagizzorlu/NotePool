using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NotePool.Application.Features.Queries.Note.SearchNotes;

namespace NotePool.Application.Features.Queries.Note.SearchNotes
{
    public class SearchNotesQueryHandler : IRequestHandler<SearchNotesQueryRequest, SearchNotesQueryResponse>
    {
        private readonly INoteReadRepository _noteReadRepository;

        public SearchNotesQueryHandler(INoteReadRepository noteReadRepository)
        {
            _noteReadRepository = noteReadRepository;
        }

        public async Task<SearchNotesQueryResponse> Handle(SearchNotesQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _noteReadRepository.GetAll(false);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(n =>
                    n.Title.Contains(request.SearchTerm) ||
                    (n.Description != null && n.Description.Contains(request.SearchTerm)) ||
                    (n.Tags != null && n.Tags.Contains(request.SearchTerm))
                );
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var noteList = await query
                .Include(n => n.User)
                .Include(n => n.Comments)
                // .Include(n => n.Reactions) // (Aklımda, Reaction entity'si bitince)
                .OrderByDescending(n => n.CreatedDate)
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .Select(note => new
                {
                    note.Id,
                    note.Title,
                    DescriptionSnippet = note.Description.Length > 150
                                            ? note.Description.Substring(0, 150) + "..."
                                            : note.Description,
                    note.CreatedDate,

                    note.ViewCount,
                    note.DownloadCount,

                    CommentCount = note.Comments.Count(),
                    // LikeCount = 0, // (Aklımda)

                    note.UserId,
                    AuthorName = note.User.UserName
                })
                .ToListAsync(cancellationToken);

            return new SearchNotesQueryResponse
            {
                Notes = noteList,
                TotalCount = totalCount
            };
        }
    }
}
