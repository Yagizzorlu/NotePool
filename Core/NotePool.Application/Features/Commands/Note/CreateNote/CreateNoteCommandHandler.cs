using MediatR;
using Microsoft.AspNetCore.Http;
using NotePool.Application.Abstractions.Hubs;
using NotePool.Application.Abstractions.Storage;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities;
using System;
using System.Linq;
using System.Security.Claims;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserReadRepository _userReadRepository;
        readonly INoteHubService _noteHubService;

        public CreateNoteCommandHandler(
            INoteWriteRepository noteWriteRepository,
            INotePdfFileWriteRepository notePdfFileWriteRepository,
            IStorageService storageService,
            IHttpContextAccessor httpContextAccessor,
            IUserReadRepository userReadRepository,
            INoteHubService noteHubService)
        {
            _noteWriteRepository = noteWriteRepository;
            _notePdfFileWriteRepository = notePdfFileWriteRepository;
            _storageService = storageService;
            _httpContextAccessor = httpContextAccessor;
            _userReadRepository = userReadRepository;
            _noteHubService = noteHubService;
        }

        public async Task<CreateNoteCommandResponse> Handle(CreateNoteCommandRequest request, CancellationToken cancellationToken)
        {
            if (request.Files == null || request.Files.Count == 0)
                throw new ArgumentException("En az bir PDF dosyası yüklemelisiniz.");

            var loggedInUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdString))
                throw new Exception("Kullanıcı girişi bulunamadı. (Token [Jeton] yok veya geçersiz).");

            var user = await _userReadRepository.GetByIdAsync(loggedInUserIdString);
            if (user == null)
                throw new Exception("Notu yükleyen kullanıcı profili veritabanında bulunamadı.");

            var uploaded = await _storageService.UploadAsync("note-files", request.Files);

            var note = new N.Note
            {
                Title = request.Title,
                Description = request.Description,
                Tags = request.Tags,
                CourseId = request.CourseId,
                UserId = user.Id,
                InstitutionId = user.InstitutionId,
                DepartmentId = user.DepartmentId
            };

            await _noteWriteRepository.AddAsync(note);

            var pdfFiles = uploaded.Select(u => new N.NotePdfFile
            {
                NoteId = note.Id,
                FileName = u.fileName,
                Path = u.pathOrContainerName,
                Storage = _storageService.StorageName
            }).ToList();

            await _notePdfFileWriteRepository.AddRangeAsync(pdfFiles);

            await _noteWriteRepository.SaveAsync();

            await _noteHubService.NoteCreatedMessageAsync($"{request.Title} adında Not Yüklenmiştir.");

            return new CreateNoteCommandResponse
            {
                NoteId = note.Id,
                Success = true,
                Message = "Not ve PDF'ler başarıyla yüklendi."
            };
        }
    }
}