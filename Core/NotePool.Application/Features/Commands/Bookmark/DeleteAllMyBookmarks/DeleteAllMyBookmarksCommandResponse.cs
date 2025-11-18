namespace NotePool.Application.Features.Commands.Bookmark.DeleteAllMyBookmarks
{
    public class DeleteAllMyBookmarksCommandResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public int DeletedCount { get; set; }
    }
}
