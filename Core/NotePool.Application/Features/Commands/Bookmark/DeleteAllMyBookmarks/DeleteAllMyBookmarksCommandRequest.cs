using MediatR;
using NotePool.Application.Features.Commands.Bookmark.DeleteAllMyBookmarks;

namespace NotePool.Application.Features.Commands.Bookmark.DeleteAllMyBookmarks
{
    public class DeleteAllMyBookmarksCommandRequest : IRequest<DeleteAllMyBookmarksCommandResponse>
    {
    }
}
