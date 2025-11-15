using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NotePool.Application.Features.Queries.Note.GetByCourseId;


namespace NotePool.Application.Features.Queries.Note.GetByCourseId
{
    public class GetByCourseIdQueryHandler : IRequestHandler<GetByCourseIdQueryRequest, GetByCourseIdQueryResponse>
    {
        private readonly INoteReadRepository _noteReadRepository;

        public GetByCourseIdQueryHandler(INoteReadRepository noteReadRepository)
        {
            _noteReadRepository = noteReadRepository;
        }

        public async Task<GetByCourseIdQueryResponse> Handle(GetByCourseIdQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _noteReadRepository.GetAll(false);

            var filteredQuery = query.Where(n => n.CourseId == request.CourseId);

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

            return new GetByCourseIdQueryResponse
            {
                Notes = noteList,
                TotalCount = totalCount
            };
        }
    }
}
