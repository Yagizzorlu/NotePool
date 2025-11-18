using MediatR;
using System;

namespace NotePool.Application.Features.Commands.NoteDownload.CreateNoteDownload
{
    public class CreateNoteDownloadCommandRequest : IRequest<CreateNoteDownloadCommandResponse>
    {
        public Guid NoteId { get; set; }
    }
}
