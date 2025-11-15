using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Institution.RemoveInstitution
{
    public class RemoveInstitutionCommandRequest : IRequest<RemoveInstitutionCommandResponse>
    {
        public string Id { get; set; }
    }
}
