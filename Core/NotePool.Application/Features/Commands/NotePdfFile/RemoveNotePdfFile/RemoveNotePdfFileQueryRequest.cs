using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.NotePdfFile.RemoveNotePdfFile
{
    public class RemoveNotePdfFileCommandRequest : IRequest<RemoveNotePdfFileCommandResponse>
    {
        public string Id { get; set; }
        public string? FileId { get; set; }
    }
}
