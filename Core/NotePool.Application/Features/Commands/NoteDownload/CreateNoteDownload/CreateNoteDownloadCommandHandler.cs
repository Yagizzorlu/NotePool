using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.NoteDownload.CreateNoteDownload
{
    public class CreateNoteDownloadCommandHandler : IRequestHandler<CreateNoteDownloadCommandRequest, CreateNoteDownloadCommandResponse>
    {
        private readonly INoteDownloadWriteRepository _noteDownloadWriteRepository;
        private readonly INoteWriteRepository _noteWriteRepository;
        private readonly INotePdfFileReadRepository _notePdfFileReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateNoteDownloadCommandHandler(
            INoteDownloadWriteRepository noteDownloadWriteRepository,
            INoteWriteRepository noteWriteRepository,
            INotePdfFileReadRepository notePdfFileReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _noteDownloadWriteRepository = noteDownloadWriteRepository;
            _noteWriteRepository = noteWriteRepository;
            _notePdfFileReadRepository = notePdfFileReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateNoteDownloadCommandResponse> Handle(CreateNoteDownloadCommandRequest request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new UnauthorizedAccessException("Oturum açmanız gerekiyor.");
            }

            var loggedInUserId = Guid.Parse(userIdString);

            var noteDownload = new Domain.Entities.NoteDownload { NoteId = request.NoteId, UserId = loggedInUserId };
            await _noteDownloadWriteRepository.AddAsync(noteDownload);

            var noteToUpdate = await _noteWriteRepository.Table.FirstOrDefaultAsync(n => n.Id == request.NoteId);
            if (noteToUpdate == null) throw new Exception("İndirilmek istenen not bulunamadı.");

            noteToUpdate.DownloadCount++;
            _noteWriteRepository.Update(noteToUpdate);
            await _noteDownloadWriteRepository.SaveAsync();

            var pdfFile = await _notePdfFileReadRepository.Table.FirstOrDefaultAsync(f => f.NoteId == request.NoteId);
            if (pdfFile == null) throw new Exception("Nota ait dosya yolu bulunamadı.");

            if (!System.IO.File.Exists(pdfFile.Path))
            {
                throw new FileNotFoundException("Dosya sunucuda bulunamadı.");
            }

            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(pdfFile.Path, cancellationToken);

            return new CreateNoteDownloadCommandResponse
            {
                IsSuccess = true,
                FileContents = fileBytes,
                FileName = pdfFile.FileName
            };
        }
    }
}
