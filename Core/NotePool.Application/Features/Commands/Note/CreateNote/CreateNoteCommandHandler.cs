using MediatR;
using NotePool.Application.Abstractions.Storage;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using N = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Commands.Note.CreateNote
{
    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommandRequest, CreateNoteCommandResponse>
    {
        private readonly INoteWriteRepository _noteWriteRepository;
        private readonly INotePdfFileWriteRepository _notePdfFileWriteRepository;
        private readonly IStorageService _storageService;

        public CreateNoteCommandHandler(
            INoteWriteRepository noteWriteRepository,
            INotePdfFileWriteRepository notePdfFileWriteRepository,
            IStorageService storageService)
        {
            _noteWriteRepository = noteWriteRepository;
            _notePdfFileWriteRepository = notePdfFileWriteRepository;
            _storageService = storageService;
        }

        public async Task<CreateNoteCommandResponse> Handle(CreateNoteCommandRequest request, CancellationToken cancellationToken)
        {
            if (request.Files == null || request.Files.Count == 0)
                throw new ArgumentException("En az bir PDF dosyası yüklemelisiniz.");

            var note = new N.Note
            {
                Title = request.Title,
                Description = request.Description,
                Tags = request.Tags,
                CourseId = request.CourseId,

                UserId = request.UserId,
                InstitutionId = request.InstitutionId,
                DepartmentId = request.DepartmentId
            };

            await _noteWriteRepository.AddAsync(note);
            await _noteWriteRepository.SaveAsync();

            var uploaded = await _storageService.UploadAsync("note-files", request.Files);

            var pdfFiles = uploaded.Select(u => new N.NotePdfFile
            {
                NoteId = note.Id,
                FileName = u.fileName,
                Path = u.pathOrContainerName,
                Storage = _storageService.StorageName
            }).ToList();

            await _notePdfFileWriteRepository.AddRangeAsync(pdfFiles);
            await _notePdfFileWriteRepository.SaveAsync();

            return new CreateNoteCommandResponse
            {
                NoteId = note.Id,
                Success = true,
                Message = "Not ve PDF'ler başarıyla yüklendi. (GEÇİCİ)"
            };
        }
    }
}