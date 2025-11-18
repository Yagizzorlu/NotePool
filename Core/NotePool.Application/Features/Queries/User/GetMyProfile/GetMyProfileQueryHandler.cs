using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.User.GetMyProfile
{
    public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQueryRequest, GetMyProfileQueryResponse>
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyProfileQueryHandler(IUserReadRepository userReadRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userReadRepository = userReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetMyProfileQueryResponse> Handle(GetMyProfileQueryRequest request, CancellationToken cancellationToken)
        {
            var loggedInUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdString))
            {
                throw new UnauthorizedAccessException("Oturum açmanız gerekiyor.");
            }

            var loggedInUserId = Guid.Parse(loggedInUserIdString);

            var profileData = await _userReadRepository.GetAll(false)
                .Include(u => u.Institution)
                .Include(u => u.Department)
                .Where(u => u.Id == loggedInUserId)
                .Select(user => new GetMyProfileQueryResponse
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    ProfileImage = user.ProfileImage,
                    InstitutionId = user.InstitutionId,
                    DepartmentId = user.DepartmentId,
                    InstitutionName = user.Institution.Name,
                    DepartmentName = user.Department.Name,

                    NotesCount = user.Notes.Count(),
                    CommentsCount = user.Comments.Count(c => c.ParentId == null),
                    RepliesCount = user.Comments.Count(c => c.ParentId != null),
                    BookmarksCount = user.Bookmarks.Count(),
                    ReactionsCount = user.Reactions.Count(),
                    DownloadsCount = user.NoteDownloads.Count() 
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (profileData == null)
            {
                throw new Exception("Kullanıcı profili veritabanında bulunamadı.");
            }

            return profileData;
        }
    }
}
