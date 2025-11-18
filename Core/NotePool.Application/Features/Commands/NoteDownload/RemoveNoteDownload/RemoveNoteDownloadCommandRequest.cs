using MediatR;
using System;

namespace NotePool.Application.Features.Commands.Download.RemoveNoteDownload
{
    public class RemoveNoteDownloadCommandRequest : IRequest<RemoveNoteDownloadCommandResponse>
    {
        public Guid Id { get; set; }
    }
}