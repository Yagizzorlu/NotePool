using MediatR;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using I = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Commands.Institution.UpdateInstitution
{
    public class UpdateInstitutionCommandHandler : IRequestHandler<UpdateInstitutionCommandRequest, UpdateInstitutionCommandResponse>
    {
        IInstitutionReadRepository _institutionReadRepository;
        IInstitutionWriteRepository _institutionWriteRepository;

        public UpdateInstitutionCommandHandler(IInstitutionReadRepository institutionReadRepository, IInstitutionWriteRepository institutionWriteRepository)
        {
            _institutionReadRepository = institutionReadRepository;
            _institutionWriteRepository = institutionWriteRepository;
        }

        public async Task<UpdateInstitutionCommandResponse> Handle(UpdateInstitutionCommandRequest request, CancellationToken cancellationToken)
        {
            I.Institution institution = await _institutionReadRepository.GetByIdAsync(request.Id);
            institution.Name = request.Name;
            institution.City = request.City;
            _institutionWriteRepository.Update(institution);
            await _institutionWriteRepository.SaveAsync();
            return new();
        }
    }
}
