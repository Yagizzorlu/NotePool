using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using NotePool.Application.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NotePool.Application.Features.Queries.Comment.GetCommentsByUserId;

namespace NotePool.Application.Features.Queries.Comment.GetCommentsByUserId
{
    public class GetCommentsByUserIdQueryHandler : IRequestHandler<GetCommentsByUserIdQueryRequest, GetCommentsByUserIdQueryResponse>
    {
        private readonly ICommentReadRepository _commentReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetCommentsByUserIdQueryHandler(
            ICommentReadRepository commentReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _commentReadRepository = commentReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetCommentsByUserIdQueryResponse> Handle(GetCommentsByUserIdQueryRequest request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return new GetCommentsByUserIdQueryResponse { TotalCount = 0, Comments = null };
            }

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

            return new GetCommentsByUserIdQueryResponse
            {
                Comments = commentList,
                TotalCount = totalCount
            };
        }
    }
}
