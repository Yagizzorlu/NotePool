using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Course.AddCourse
{
    public class AddCourseCommandRequest : IRequest<AddCourseCommandResponse>
    {
        public string Name { get; set; }
        public string? Code { get; set; }
        public int Year { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
