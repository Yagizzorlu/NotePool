namespace NotePool.Application.Features.Commands.NoteDownload.CreateNoteDownload
{
    public class CreateNoteDownloadCommandResponse
    {
        public bool IsSuccess { get; set; }
        public byte[] FileContents { get; set; }
        public string FileName { get; set; }
    }
}
