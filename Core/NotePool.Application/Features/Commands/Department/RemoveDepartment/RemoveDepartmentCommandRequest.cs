using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Department.RemoveDepartment
{
    public class RemoveDepartmentCommandRequest : IRequest<RemoveDepartmentCommandResponse>
    {
        public string Id { get; set; }
    }
}
