using MediatR;
using Microsoft.AspNetCore.Http;
using NotePool.Application.Abstractions.Storage;
using NotePool.Application.Features.Commands.NotePdfFile.UploadNotePdfFile.NotePool.Application.Features.Commands.NotePdfFile.UploadNotePdfFile;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UploadNotePdfFileCommandHandler(
            INotePdfFileWriteRepository notePdfFileWriteRepository,
            IStorageService storageService,
            INoteReadRepository noteReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _notePdfFileWriteRepository = notePdfFileWriteRepository;
            _storageService = storageService;
            _noteReadRepository = noteReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UploadNotePdfFileCommandResponse> Handle(UploadNotePdfFileCommandRequest request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
                throw new UnauthorizedAccessException("Oturum açmanız gerekiyor.");
            var loggedInUserId = Guid.Parse(userIdString);

            N.Note note = await _noteReadRepository.GetByIdAsync(request.NoteId.ToString());
            if (note == null)
                throw new KeyNotFoundException("İlgili not bulunamadı.");

            if (note.UserId != loggedInUserId)
                throw new UnauthorizedAccessException("Bu nota dosya yükleme yetkiniz yok.");

            if (request.Files == null || request.Files.Count == 0)
                throw new ArgumentException("En az bir PDF dosyası yüklemelisiniz.", nameof(request.Files));

            foreach (var f in request.Files)
            {
                var isPdf = string.Equals(f.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase)
                                 || string.Equals(System.IO.Path.GetExtension(f.FileName), ".pdf", StringComparison.OrdinalIgnoreCase);
                if (!isPdf)
                    throw new ArgumentException($"Sadece PDF dosyaları yüklenebilir. Hatalı dosya: {f.FileName}");
            }

            List<(string fileName, string pathOrContainerName)> storageResult =
                await _storageService.UploadAsync("note-files", request.Files);

            if (storageResult == null || storageResult.Count == 0)
                throw new InvalidOperationException("Dosyalar yüklenirken bir hata oluştu.");

            var pdfEntities = storageResult.Select(r => new N.NotePdfFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                NoteId = note.Id,
            }).ToList();

            await _notePdfFileWriteRepository.AddRangeAsync(pdfEntities);
            await _notePdfFileWriteRepository.SaveAsync();

            var uploadedDetails = pdfEntities.Select(e => new
            {
                e.Id,
                e.FileName,
                e.Path
            }).ToList();

            return new UploadNotePdfFileCommandResponse
            {
                IsSuccess = true,
                UploadedFiles = uploadedDetails
            };
        }
    }
}