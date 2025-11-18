using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.IO; // Dosya silme (I/O) için gerekli
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using N = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Commands.NotePdfFile.RemoveNotePdfFile
{
    public class RemoveNotePdfFileCommandHandler : IRequestHandler<RemoveNotePdfFileCommandRequest, RemoveNotePdfFileCommandResponse>
    {
        readonly INoteReadRepository _noteReadRepository;
        readonly INoteWriteRepository _noteWriteRepository;

        public RemoveNotePdfFileCommandHandler(INoteReadRepository noteReadRepository, INoteWriteRepository noteWriteRepository)
        {
            _noteReadRepository = noteReadRepository;
            _noteWriteRepository = noteWriteRepository;
        }

        public async Task<RemoveNotePdfFileCommandResponse> Handle(RemoveNotePdfFileCommandRequest request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.Id, out Guid noteId))
            {
                throw new ArgumentException("Not ID formatı geçersiz.");
            }
            if (!Guid.TryParse(request.FileId, out Guid fileId))
            {
                throw new ArgumentException("Dosya ID formatı geçersiz.");
            }

            N.Note? note = await _noteReadRepository.Table.Include(n => n.NotePdfFiles)
                .FirstOrDefaultAsync(n => n.Id == noteId, cancellationToken);

            if (note == null)
            {
                throw new Exception("İlgili not bulunamadı.");
            }

            N.NotePdfFile? notePdfFile = note.NotePdfFiles.FirstOrDefault(n => n.Id == fileId);

            if (notePdfFile != null)
            {
                string filePath = notePdfFile.Path;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                note.NotePdfFiles.Remove(notePdfFile);
            }

            await _noteWriteRepository.SaveAsync();

            return new RemoveNotePdfFileCommandResponse
            {

            };
        }
    }
}