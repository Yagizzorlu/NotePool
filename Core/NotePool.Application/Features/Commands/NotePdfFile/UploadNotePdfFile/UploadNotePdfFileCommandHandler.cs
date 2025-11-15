using MediatR;
using NotePool.Application.Abstractions.Storage;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using N = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Commands.NotePdfFile.UploadNotePdfFile
{
    public class UploadNotePdfFileCommandHandler
        : IRequestHandler<UploadNotePdfFileCommandRequest, UploadNotePdfFileCommandResponse>
    {
        private readonly IStorageService _storageService;
        private readonly INoteReadRepository _noteReadRepository;
        private readonly INotePdfFileWriteRepository _notePdfFileWriteRepository;

        public UploadNotePdfFileCommandHandler(
            INotePdfFileWriteRepository notePdfFileWriteRepository,
            IStorageService storageService,
            INoteReadRepository noteReadRepository)
        {
            _notePdfFileWriteRepository = notePdfFileWriteRepository;
            _storageService = storageService;
            _noteReadRepository = noteReadRepository;
        }

        public async Task<UploadNotePdfFileCommandResponse> Handle(
            UploadNotePdfFileCommandRequest request,
            CancellationToken cancellationToken)
        {
            // 1) Dosya kontrolü
            if (request.Files == null || request.Files.Count == 0)
                throw new ArgumentException("En az bir PDF dosyası yüklemelisiniz.", nameof(request.Files));

            // (Opsiyonel) Yalnızca PDF kabul et
            // İstersen bu kontrolü FluentValidation'a da taşıyabilirsin.
            foreach (var f in request.Files)
            {
                var isPdf = string.Equals(f.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(System.IO.Path.GetExtension(f.FileName), ".pdf", StringComparison.OrdinalIgnoreCase);
                if (!isPdf)
                    throw new ArgumentException("Sadece PDF dosyaları yüklenebilir.");
            }

            // 2) Not mevcut mu?
            // GetByIdAsync imzan string alıyorsa bu satır doğru; Guid alıyorsa Guid.Parse kullan.
            N.Note note = await _noteReadRepository.GetByIdAsync(request.Id);
            if (note == null)
                throw new KeyNotFoundException("Note bulunamadı.");

            List<(string fileName, string pathOrContainerName)> result =
                await _storageService.UploadAsync("note-files", request.Files);

            if (result == null || result.Count == 0)
                throw new InvalidOperationException("Dosyalar yüklenemedi.");

            var pdfEntities = result.Select(r => new Domain.Entities.NotePdfFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,   
                Storage = _storageService.StorageName, 
                NoteId = note.Id
            }).ToList();

            await _notePdfFileWriteRepository.AddRangeAsync(pdfEntities);
            await _notePdfFileWriteRepository.SaveAsync();
            var response = new UploadNotePdfFileCommandResponse();

            return response;
        }
    }
}