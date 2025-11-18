using MediatR;
using NotePool.Application.Features.Queries.Comment.GetCommentsByUserId;
using System;

namespace NotePool.Application.Features.Queries.Comment.GetCommentsByUserId
{
    public class GetCommentsByUserIdQueryRequest : IRequest<GetCommentsByUserIdQueryResponse>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}
