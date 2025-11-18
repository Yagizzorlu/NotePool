using MediatR;
using NotePool.Application.Features.Queries.Comment.GetMyComments;

namespace NotePool.Application.Features.Queries.Comment.GetMyComments
{
    public class GetMyCommentsQueryRequest : IRequest<GetMyCommentsQueryResponse>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}