using MediatR;
using NotePool.Application.Repositories;
using System.Threading;
using System.Threading.Tasks;
using N = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Commands.Note.UpdateNote
{
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommandRequest, UpdateNoteCommandResponse>
    {
        readonly INoteReadRepository _noteReadRepository;
        readonly INoteWriteRepository _noteWriteRepository; 

        public UpdateNoteCommandHandler(
            INoteReadRepository noteReadRepository,
            INoteWriteRepository noteWriteRepository) 
        {
            _noteReadRepository = noteReadRepository;
            _noteWriteRepository = noteWriteRepository;
        }

        public async Task<UpdateNoteCommandResponse> Handle(UpdateNoteCommandRequest request, CancellationToken cancellationToken)
        {
            N.Note note = await _noteReadRepository.GetByIdAsync(request.Id);

            if (note == null)
            {
                throw new Exception("Güncellenecek not bulunamadı.");
            }
            note.Description = request.Description;
            note.Title = request.Title;
            note.Tags = request.Tags;

            _noteWriteRepository.Update(note);
            await _noteWriteRepository.SaveAsync();
            return new();
        }
    }
}
