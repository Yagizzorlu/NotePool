using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Note.CreateNote
{
    public class CreateNoteCommandRequest : IRequest<CreateNoteCommandResponse>
    {
            public string Title { get; set; }
            public string? Description { get; set; }
            public string? Tags { get; set; }
            public Guid CourseId { get; set; }
            public IFormFileCollection Files { get; set; }
        }
    }
