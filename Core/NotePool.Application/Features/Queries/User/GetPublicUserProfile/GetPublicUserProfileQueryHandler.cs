using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.User.GetPublicUserProfile
{
    public class GetPublicUserProfileQueryHandler : IRequestHandler<GetPublicUserProfileQueryRequest, GetPublicUserProfileQueryResponse>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetPublicUserProfileQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<GetPublicUserProfileQueryResponse> Handle(GetPublicUserProfileQueryRequest request, CancellationToken cancellationToken)
        {
            var user = await _userReadRepository.GetAll(false)
                .Include(u => u.Institution)
                .Include(u => u.Department)
                .Include(u => u.Notes)
                .Include(u => u.Bookmarks)
                .Include(u => u.Comments)
                .Include(u => u.Reactions)
                .Include(u => u.NoteDownloads) 
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(request.Id), cancellationToken);

            if (user == null)
            {
                return null;
            }

            return new GetPublicUserProfileQueryResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileImage = user.ProfileImage,
                InstitutionName = user.Institution?.Name,
                DepartmentName = user.Department?.Name,

                NotesCount = user.Notes.Count(),
                CommentsCount = user.Comments.Count(c => c.ParentId == null),
                RepliesCount = user.Comments.Count(c => c.ParentId != null),
                ReactionsCount = user.Reactions.Count(),
                BookmarksCount = user.Bookmarks.Count(),
                DownloadsCount = user.NoteDownloads.Count() 
            };
        }
    }
}
