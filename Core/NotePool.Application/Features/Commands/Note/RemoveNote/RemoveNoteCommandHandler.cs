using MediatR;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Note.RemoveNote
{
    public class RemoveNoteCommandHandler : IRequestHandler<RemoveNoteCommandRequest, RemoveNoteCommandResponse>
    {
        readonly INoteWriteRepository _noteWriteRepository;

        public RemoveNoteCommandHandler(INoteWriteRepository noteWriteRepository)
        {
            _noteWriteRepository = noteWriteRepository;
        }

        public async Task<RemoveNoteCommandResponse> Handle(RemoveNoteCommandRequest request, CancellationToken cancellationToken)
        {
            await _noteWriteRepository.RemoveAsync(request.Id);
            await _noteWriteRepository.SaveAsync();
            return new();
        }
    }
}
