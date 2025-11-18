using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NotePool.Domain.Entities.Enums;

namespace NotePool.Application.Features.Queries.User.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQueryRequest, GetUserByIdQueryResponse>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetUserByIdQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<GetUserByIdQueryResponse> Handle(GetUserByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var user = await _userReadRepository.GetAll(false)
                .Include(u => u.Institution)
                .Include(u => u.Department)
                .Include(u => u.Notes)
                .Include(u => u.Comments)
                .Include(u => u.Reactions)
                .Include(u => u.Bookmarks)
                .Include(u => u.NoteDownloads)
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(request.Id), cancellationToken);

            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            return new GetUserByIdQueryResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                ProfileImage = user.ProfileImage,
                InstitutionId = user.InstitutionId,
                DepartmentId = user.DepartmentId,
                InstitutionName = user.Institution?.Name,
                DepartmentName = user.Department?.Name,

                NoteCount = user.Notes.Count(),
                CommentCount = user.Comments.Count(c => c.ParentId == null),
                ReplyCount = user.Comments.Count(c => c.ParentId != null),
                ReactionsCount = user.Reactions.Count(),
                BookmarksCount = user.Bookmarks.Count(),
                DownloadsCount = user.NoteDownloads.Count(),
                LikeCount = user.Reactions.Count(r => r.Type == ReactionType.Like)
            };
        }
    }
}
