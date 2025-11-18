using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NotePool.Domain.Entities.Enums;

namespace NotePool.Application.Features.Queries.User.GetUserByInstitutionId
{
    public class GetUserByInstitutionIdQueryHandler : IRequestHandler<GetUserByInstitutionIdQueryRequest, GetUserByInstitutionIdQueryResponse>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetUserByInstitutionIdQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<GetUserByInstitutionIdQueryResponse> Handle(GetUserByInstitutionIdQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _userReadRepository.GetAll(false);

            var filteredQuery = query.Where(u => u.InstitutionId == request.InstitutionId);

            var totalCount = await filteredQuery.CountAsync(cancellationToken);

            var usersList = await filteredQuery
                .Include(u => u.Department)
                .Include(u => u.Notes)
                .Include(u => u.Comments)
                .Include(u => u.Reactions)
                .Include(u => u.Bookmarks)
                .Include(u => u.NoteDownloads) 
                .OrderBy(u => u.UserName)
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .Select(user => new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    DepartmentName = user.Department != null ? user.Department.Name : "Atanmamış",

                    NotesCount = user.Notes.Count(),
                    CommentCount = user.Comments.Count(c => c.ParentId == null),
                    ReplyCount = user.Comments.Count(c => c.ParentId != null),
                    ReactionsCount = user.Reactions.Count(),
                    BookmarksCount = user.Bookmarks.Count(),
                    DownloadsCount = user.NoteDownloads.Count(),
                    LikeCount = user.Reactions.Count(r => r.Type == ReactionType.Like)
                })
                .ToListAsync(cancellationToken);

            return new GetUserByInstitutionIdQueryResponse
            {
                Users = usersList,
                TotalCount = totalCount
            };
        }
    }
}
