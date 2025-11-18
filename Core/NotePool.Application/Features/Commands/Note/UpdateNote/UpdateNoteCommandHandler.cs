using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NotePool.Application.Repositories;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using N = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Commands.Note.UpdateNote
{
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommandRequest, UpdateNoteCommandResponse>
    {
        readonly INoteReadRepository _noteReadRepository;
        readonly INoteWriteRepository _noteWriteRepository;
        readonly ILogger<UpdateNoteCommandHandler> _logger;
        readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateNoteCommandHandler(
            INoteReadRepository noteReadRepository,
            INoteWriteRepository noteWriteRepository,
            ILogger<UpdateNoteCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _noteReadRepository = noteReadRepository;
            _noteWriteRepository = noteWriteRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UpdateNoteCommandResponse> Handle(UpdateNoteCommandRequest request, CancellationToken cancellationToken)
        {
            var loggedInUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdString))
            {
                throw new Exception("Kullanıcı girişi bulunamadı (Token [Jeton] yok veya geçersiz).");
            }

            N.Note note = await _noteReadRepository.GetByIdAsync(request.Id);

            if (note == null)
            {
                throw new Exception("Güncellenecek not bulunamadı.");
            }

            if (note.UserId.ToString() != loggedInUserIdString)
            {
                throw new Exception("Bu notu güncellemeye yetkiniz yok.");
            }

            note.Description = request.Description;
            note.Title = request.Title;
            note.Tags = request.Tags;

            _noteWriteRepository.Update(note);
            await _noteWriteRepository.SaveAsync();
            _logger.LogInformation("NOT GÜNCELLENDİ");

            return new();
        }
    }
}
