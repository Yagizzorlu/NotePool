using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Note.CreateNote
{
    public class CreateNoteCommandResponse
    {
        public bool Success { get; set; }
        public string Message {  get; set; } 
        public Guid NoteId { get; set; }
    }
}
