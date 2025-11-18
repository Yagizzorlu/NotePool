using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using NotePool.Application.Features.Queries.Comment.GetMyComments;

namespace NotePool.Application.Features.Queries.Comment.GetMyComments
{
    public class GetMyCommentsQueryHandler : IRequestHandler<GetMyCommentsQueryRequest, GetMyCommentsQueryResponse>
    {
        private readonly ICommentReadRepository _commentReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyCommentsQueryHandler(
            ICommentReadRepository commentReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _commentReadRepository = commentReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetMyCommentsQueryResponse> Handle(GetMyCommentsQueryRequest request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                throw new UnauthorizedAccessException("Kullanıcı girişi bulunamadı.");

            var currentUserId = Guid.Parse(userIdString);

            var query = _commentReadRepository.GetAll(false)
                .Where(c => c.UserId == currentUserId);

            var totalCount = await query.CountAsync(cancellationToken);

            var commentList = await query
                .Include(c => c.Note)
                .Include(c => c.Parent) 
                    .ThenInclude(p => p.User)
                .OrderByDescending(c => c.CreatedDate)
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .Select(comment => new
                {
                    comment.Id,
                    comment.Content,
                    comment.CreatedDate,

                    comment.NoteId,
                    NoteTitle = comment.Note != null ? comment.Note.Title : "Silinmiş Not",

                    IsReply = comment.ParentId != null,
                    ParentId = comment.ParentId,

                    RepliedToUserName = (comment.Parent != null && comment.Parent.User != null)
                                        ? comment.Parent.User.UserName
                                        : null
                })
                .ToListAsync(cancellationToken);

            return new GetMyCommentsQueryResponse
            {
                Comments = commentList,
                TotalCount = totalCount
            };
        }
    }
}