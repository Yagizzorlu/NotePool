using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.User.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQueryRequest, GetAllUsersQueryResponse>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetAllUsersQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _userReadRepository.GetAll(false);

            var totalCount = await query.CountAsync(cancellationToken);

            var usersList = await query
                .Include(u => u.Institution)
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
                    InstitutionName = user.Institution != null ? user.Institution.Name : "Atanmamış",
                    DepartmentName = user.Department != null ? user.Department.Name : "Atanmamış",


                    NotesCount = user.Notes.Count(),
                    CommentsCount = user.Comments.Count(c => c.ParentId == null), 
                    RepliesCount = user.Comments.Count(c => c.ParentId != null), 
                    BookmarksCount = user.Bookmarks.Count(),
                    ReactionsCount = user.Reactions.Count(),
                    DownloadsCount = user.NoteDownloads.Count() 
                })
                .ToListAsync(cancellationToken);

            return new GetAllUsersQueryResponse
            {
                Users = usersList,
                TotalCount = totalCount
            };
        }
    }
}