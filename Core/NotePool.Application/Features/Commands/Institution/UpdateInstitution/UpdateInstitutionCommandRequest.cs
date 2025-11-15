using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Institution.UpdateInstitution
{
    public class UpdateInstitutionCommandRequest : IRequest<UpdateInstitutionCommandResponse>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? City { get; set; }
    }
}
