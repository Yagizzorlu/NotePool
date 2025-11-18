using MediatR;
using Microsoft.AspNetCore.Http;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Bookmark.BookmarkNote
{
    public class BookmarkNoteCommandHandler : IRequestHandler<BookmarkNoteCommandRequest, BookmarkNoteCommandResponse>
    {
        private readonly IBookmarkReadRepository _bookmarkReadRepository;
        private readonly IBookmarkWriteRepository _bookmarkWriteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookmarkNoteCommandHandler(IBookmarkReadRepository bookmarkReadRepository, IBookmarkWriteRepository bookmarkWriteRepository, IHttpContextAccessor httpContextAccessor)
        {
            _bookmarkReadRepository = bookmarkReadRepository;
            _bookmarkWriteRepository = bookmarkWriteRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BookmarkNoteCommandResponse> Handle(BookmarkNoteCommandRequest request, CancellationToken cancellationToken)
        {
            var loggedInUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(loggedInUserIdString))
            {
                throw new Exception("Kullanıcı Girişi Bulunamadı");
            }

            var loggedInUserId = Guid.Parse(loggedInUserIdString);
            var noteGuid = Guid.Parse(request.NoteId);

            var existingBookmark = await _bookmarkReadRepository.GetSingleAsync(b=>b.UserId == loggedInUserId && b.NoteId == noteGuid, true);

            bool isBookmarked;

            if(existingBookmark == null)
            {
                var newBookmark = new Domain.Entities.Bookmark
                {
                    Id = Guid.NewGuid(),
                    UserId = loggedInUserId,
                    NoteId = noteGuid
                };
                await _bookmarkWriteRepository.AddAsync(newBookmark);
                isBookmarked = true;
            } else
            {
                _bookmarkWriteRepository.Remove(existingBookmark);
                isBookmarked = false;
            }

            await _bookmarkWriteRepository.SaveAsync();

            return new BookmarkNoteCommandResponse
            {
                IsBookmarked = isBookmarked,
                Message = isBookmarked ? "Not Kaydedildi" : "Not Kaydedilenlerden Çıkarıldı"
            };

        }
    }
}
