using MediatR;
using NotePool.Application.Features.Queries.Note.GetByDepartmentId;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Note.GetByDepartmentId
{
    public class GetByDepartmentIdQueryHandler : IRequestHandler<GetByDepartmentIdQueryRequest, GetByDepartmentIdQueryResponse>
    {
        private readonly INoteReadRepository _noteReadRepository;

        public GetByDepartmentIdQueryHandler(INoteReadRepository noteReadRepository)
        {
            _noteReadRepository = noteReadRepository;
        }

        public async Task<GetByDepartmentIdQueryResponse> Handle(GetByDepartmentIdQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _noteReadRepository.GetAll(false);

            var filteredQuery = query.Where(n => n.CourseId == request.DepartmentId);

            var totalCount = await filteredQuery.CountAsync(cancellationToken);

            var noteList = await filteredQuery
                .Include(n => n.User)
                // .Include(n => n.Reactions) // Reaction entity'si bitince (Aklımda)
                .Include(n => n.Comments)
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
                    // LikeCount = note.Reactions.Count(), // (Aklımda)

                    note.UserId,
                    AuthorName = note.User.UserName
                })
                .ToListAsync(cancellationToken);

            return new GetByDepartmentIdQueryResponse
            {
                Notes = noteList,
                TotalCount = totalCount
            };
        }
    }
}
