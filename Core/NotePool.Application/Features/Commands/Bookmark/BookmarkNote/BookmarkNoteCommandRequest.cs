using MediatR;
using NotePool.Application.Features.Commands.Bookmark.BookmarkNote;

namespace NotePool.Application.Features.Commands.Bookmark.BookmarkNote
{
    public class BookmarkNoteCommandRequest : IRequest<BookmarkNoteCommandResponse>
    {
        public string NoteId { get; set; }
    }
}
