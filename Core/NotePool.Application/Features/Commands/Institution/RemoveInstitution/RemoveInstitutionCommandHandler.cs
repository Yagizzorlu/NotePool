using MediatR;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Institution.RemoveInstitution
{
    public class RemoveInstitutionCommandHandler : IRequestHandler<RemoveInstitutionCommandRequest, RemoveInstitutionCommandResponse>
    {
        private readonly IInstitutionWriteRepository _institutionWriteRepository;

        public RemoveInstitutionCommandHandler(IInstitutionWriteRepository institutionWriteRepository)
        {
            _institutionWriteRepository = institutionWriteRepository;
        }

        public async Task<RemoveInstitutionCommandResponse> Handle(RemoveInstitutionCommandRequest request, CancellationToken cancellationToken)
        {
            await _institutionWriteRepository.RemoveAsync(request.Id);
            await _institutionWriteRepository.SaveAsync();
            return new();
        }
    }
}
