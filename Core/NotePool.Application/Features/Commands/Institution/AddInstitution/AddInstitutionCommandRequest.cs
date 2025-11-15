using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Institution.AddInstitution
{
    public class AddInstitutionCommandRequest : IRequest<AddInstitutionCommandResponse>
    {
        public string Name { get; set; }
        public string? City { get; set; }
    }
}
