using MediatR;
using NotePool.Application.Features.Commands.Note.CreateNote;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using I = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Commands.Institution.AddInstitution
{
    public class AddInstitutionCommandHandler : IRequestHandler<AddInstitutionCommandRequest, AddInstitutionCommandResponse>
    {
        private readonly IInstitutionWriteRepository _institutionWriteRepository;

        public AddInstitutionCommandHandler(IInstitutionWriteRepository institutionWriteRepository)
        {
            _institutionWriteRepository = institutionWriteRepository;
        }

        public async Task<AddInstitutionCommandResponse> Handle(AddInstitutionCommandRequest request, CancellationToken cancellationToken)
        {
            I.Institution institution = new()
            {
                Name = request.Name,
                City = request.City,
            };

            await _institutionWriteRepository.AddAsync(institution);
            await _institutionWriteRepository.SaveAsync();

            return new AddInstitutionCommandResponse
            {
                Success = true,
                Message = "Başarıyla Yüklendi",
                Id = institution.Id
            };
        }
    }
}
