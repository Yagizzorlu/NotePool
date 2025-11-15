using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.NotePdfFile.UploadNotePdfFile
{
    public class UploadNotePdfFileCommandRequest : IRequest<UploadNotePdfFileCommandResponse>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }
        public Guid CourseId { get; set; }
        public Guid UserId { get; set; }
        public Guid InstitutionId { get; set; }
        public IFormFileCollection Files { get; set; }
    }
}
