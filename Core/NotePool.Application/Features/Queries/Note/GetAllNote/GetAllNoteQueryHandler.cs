using MediatR;
using NotePool.Application.Repositories;
using NotePool.Application.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 

namespace NotePool.Application.Features.Queries.Note.GetAllNote
{
    public class GetAllNoteQueryHandler : IRequestHandler<GetAllNoteQueryRequest, GetAllNoteQueryResponse>
    {
        readonly INoteReadRepository _noteReadRepository;

        public GetAllNoteQueryHandler(INoteReadRepository noteReadRepository)
        {
            _noteReadRepository = noteReadRepository;
        }

        public async Task<GetAllNoteQueryResponse> Handle(GetAllNoteQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _noteReadRepository.GetAll(false);
            var totalCount = await query.CountAsync(cancellationToken);
            var notes = await query
                .Include(n => n.User) 

                .Include(n => n.Comments) 
                .OrderByDescending(n => n.CreatedDate) 
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .Select(n => new 
                {
                    n.Id,
                    n.Title,
                    DescriptionSnippet = n.Description.Length > 150
                                            ? n.Description.Substring(0, 150) + "..."
                                            : n.Description,
                    n.CreatedDate,
                    n.ViewCount,
                    n.DownloadCount,

                    CommentCount = n.Comments.Count(),
                    n.UserId,
                    AuthorName = n.User.UserName 
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
