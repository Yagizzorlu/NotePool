using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Course.RemoveCourse
{
    public class RemoveCourseCommandRequest : IRequest<RemoveCourseCommandResponse>
    {
        public string Id { get; set; }
    }
}
