using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Institution.AddInstitution
{
    public class AddInstitutionCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid Id { get; set; }
    }
}
