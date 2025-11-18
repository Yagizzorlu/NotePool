using MediatR;
using NotePool.Application.Features.Queries.Bookmark.GetMyBookmarks;

namespace NotePool.Application.Features.Queries.Bookmark.GetMyBookmarks
{
    public class GetMyBookmarksQueryRequest : IRequest<GetMyBookmarksQueryResponse>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}
