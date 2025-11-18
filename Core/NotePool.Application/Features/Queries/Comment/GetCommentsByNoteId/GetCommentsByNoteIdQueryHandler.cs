using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NotePool.Application.Features.Queries.Comment.GetCommentsByNoteId;

namespace NotePool.Application.Features.Queries.Comment.GetCommentsByNoteId
{
    public class GetCommentsByNoteIdQueryHandler : IRequestHandler<GetCommentsByNoteIdQueryRequest, GetCommentsByNoteIdQueryResponse>
    {
        private readonly ICommentReadRepository _commentReadRepository;

        public GetCommentsByNoteIdQueryHandler(ICommentReadRepository commentReadRepository)
        {
            _commentReadRepository = commentReadRepository;
        }

        public async Task<GetCommentsByNoteIdQueryResponse> Handle(GetCommentsByNoteIdQueryRequest request, CancellationToken cancellationToken)
        {
            var noteGuid = Guid.Parse(request.NoteId);

            var query = _commentReadRepository.GetAll(false)
                .Where(c => c.NoteId == noteGuid && c.ParentId == null);

            var totalCount = await query.CountAsync(cancellationToken);

            var commentList = await query
                .Include(c => c.User)
                .Include(c => c.Replies)
                    .ThenInclude(r => r.User)
                .OrderByDescending(c => c.CreatedDate)
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .Select(comment => new
                {
                    comment.Id,
                    comment.Content,
                    comment.CreatedDate,
                    CommentAuthorId = comment.UserId,
                    CommentAuthorName = comment.User != null ? comment.User.UserName : "Bilinmeyen Kullanıcı",

                    Replies = comment.Replies
                        .OrderBy(r => r.CreatedDate)
                        .Select(reply => new
                        {
                            reply.Id,
                            reply.Content,
                            reply.CreatedDate,
                            ReplyAuthorId = reply.UserId,
                            ReplyAuthorName = reply.User != null ? reply.User.UserName : "Bilinmeyen Kullanıcı"
                        }).ToList()
                })
                .ToListAsync(cancellationToken);

            return new GetCommentsByNoteIdQueryResponse
            {
                Comments = commentList,
                TotalCount = totalCount
            };
        }
    }
}