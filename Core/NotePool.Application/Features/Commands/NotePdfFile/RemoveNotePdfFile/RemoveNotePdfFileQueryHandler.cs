using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
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
            N.Note? note = await _noteReadRepository.Table.Include(n => n.NotePdfFiles)
                .FirstOrDefaultAsync(n => n.Id == Guid.Parse(request.Id));

            N.NotePdfFile notePdfFile = note?.NotePdfFiles.FirstOrDefault(n => n.Id == Guid.Parse(request.FileId));

            if(notePdfFile != null)
            {
                note.NotePdfFiles.Remove(notePdfFile);
            }
            await _noteWriteRepository.SaveAsync();
            return new();
        }
    }
}
