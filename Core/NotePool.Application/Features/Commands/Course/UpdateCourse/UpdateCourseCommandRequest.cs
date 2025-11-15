using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Course.UpdateCourse
{
    public class UpdateCourseCommandRequest : IRequest<UpdateCourseCommandResponse>
    {
        public string Id { get; set; }
        public string Name { get; set;}
        public string? Code { get; set;}
        public int Year { get; set;}
        public string DepartmentId { get; set;}
    }
}
