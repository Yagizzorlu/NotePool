using MediatR;
using Microsoft.AspNetCore.Http;
using NotePool.Application.Repositories;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using N = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Commands.Note.RemoveNote
{
    public class RemoveNoteCommandHandler : IRequestHandler<RemoveNoteCommandRequest, RemoveNoteCommandResponse>
    {
        readonly INoteWriteRepository _noteWriteRepository;
        readonly INoteReadRepository _noteReadRepository;
        readonly IHttpContextAccessor _httpContextAccessor;

        public RemoveNoteCommandHandler(
            INoteWriteRepository noteWriteRepository,
            INoteReadRepository noteReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _noteWriteRepository = noteWriteRepository;
            _noteReadRepository = noteReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<RemoveNoteCommandResponse> Handle(RemoveNoteCommandRequest request, CancellationToken cancellationToken)
        {
            var loggedInUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdString))
            {
                throw new Exception("Kullanıcı girişi bulunamadı (Token [Jeton] yok veya geçersiz).");
            }

            N.Note note = await _noteReadRepository.GetByIdAsync(request.Id);

            if (note == null)
            {
                throw new Exception("Silinecek not bulunamadı.");
            }

            if (note.UserId.ToString() != loggedInUserIdString)
            {
                throw new Exception("Bu notu silmeye yetkiniz yok.");
            }

            _noteWriteRepository.Remove(note);
            await _noteWriteRepository.SaveAsync();

            return new();
        }
    }
}
