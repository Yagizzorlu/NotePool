using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Download.RemoveNoteDownload
{
    public class RemoveNoteDownloadCommandHandler : IRequestHandler<RemoveNoteDownloadCommandRequest, RemoveNoteDownloadCommandResponse>
    {
        private readonly INoteDownloadWriteRepository _noteDownloadWriteRepository;
        private readonly INoteWriteRepository _noteWriteRepository;
        private readonly INoteDownloadReadRepository _noteDownloadReadRepository;

        public RemoveNoteDownloadCommandHandler(
            INoteDownloadWriteRepository noteDownloadWriteRepository,
            INoteWriteRepository noteWriteRepository,
            INoteDownloadReadRepository noteDownloadReadRepository)
        {
            _noteDownloadWriteRepository = noteDownloadWriteRepository;
            _noteWriteRepository = noteWriteRepository;
            _noteDownloadReadRepository = noteDownloadReadRepository;
        }

        public async Task<RemoveNoteDownloadCommandResponse> Handle(RemoveNoteDownloadCommandRequest request, CancellationToken cancellationToken)
        {
            var downloadLog = await _noteDownloadReadRepository.Table
                .FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

            if (downloadLog == null)
            {
                return new RemoveNoteDownloadCommandResponse { IsSuccess = false, Message = "Silinecek indirme kaydı bulunamadı." };
            }

            Guid noteId = downloadLog.NoteId;

            await _noteDownloadWriteRepository.RemoveAsync(request.Id.ToString());

            var noteToUpdate = await _noteWriteRepository.Table.FirstOrDefaultAsync(n => n.Id == noteId);

            if (noteToUpdate != null && noteToUpdate.DownloadCount > 0)
            {
                noteToUpdate.DownloadCount--;
                _noteWriteRepository.Update(noteToUpdate);
            }

            await _noteDownloadWriteRepository.SaveAsync();

            return new RemoveNoteDownloadCommandResponse { IsSuccess = true, Message = "İndirme kaydı başarıyla silindi ve sayaç güncellendi." };
        }
    }
}
