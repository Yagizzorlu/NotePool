using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Note.UpdateNote
{
    public class UpdateNoteCommandRequest : IRequest<UpdateNoteCommandResponse>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }
    }
}
