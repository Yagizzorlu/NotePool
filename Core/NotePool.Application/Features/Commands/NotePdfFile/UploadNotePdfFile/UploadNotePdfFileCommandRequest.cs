using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.NotePdfFile.UploadNotePdfFile
{
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using System;

    namespace NotePool.Application.Features.Commands.NotePdfFile.UploadNotePdfFile
    {
        public class UploadNotePdfFileCommandRequest : IRequest<UploadNotePdfFileCommandResponse>
        {
            public Guid NoteId { get; set; }
            public IFormFileCollection Files { get; set; }
        }
    }
}
