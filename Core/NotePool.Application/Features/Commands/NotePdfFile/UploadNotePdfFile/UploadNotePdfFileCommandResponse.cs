namespace NotePool.Application.Features.Commands.NotePdfFile.UploadNotePdfFile
{
    public class UploadNotePdfFileCommandResponse
    {
        public bool IsSuccess { get; set; } = false;
        public object UploadedFiles { get; set; }
    }
}
