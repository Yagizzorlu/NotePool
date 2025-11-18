using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using NotePool.Application.Features.Commands.Bookmark.DeleteAllMyBookmarks;

namespace NotePool.Application.Features.Commands.Bookmark.DeleteAllMyBookmarks
{
    public class DeleteAllMyBookmarksCommandHandler : IRequestHandler<DeleteAllMyBookmarksCommandRequest, DeleteAllMyBookmarksCommandResponse>
    {
        private readonly IBookmarkReadRepository _bookmarkReadRepository;
        private readonly IBookmarkWriteRepository _bookmarkWriteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteAllMyBookmarksCommandHandler(
            IBookmarkReadRepository bookmarkReadRepository,
            IBookmarkWriteRepository bookmarkWriteRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _bookmarkReadRepository = bookmarkReadRepository;
            _bookmarkWriteRepository = bookmarkWriteRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DeleteAllMyBookmarksCommandResponse> Handle(DeleteAllMyBookmarksCommandRequest request, CancellationToken cancellationToken)
        {
            var loggedInUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdString))
                throw new Exception("Kullanıcı girişi bulunamadı.");

            var loggedInUserId = Guid.Parse(loggedInUserIdString);

            var bookmarksToDelete = await _bookmarkReadRepository.GetAll(false)
                .Where(b => b.UserId == loggedInUserId)
                .ToListAsync(cancellationToken);

            int count = bookmarksToDelete.Count;

            if (count > 0)
            {
                _bookmarkWriteRepository.RemoveRange(bookmarksToDelete);
                await _bookmarkWriteRepository.SaveAsync();
            }

            return new DeleteAllMyBookmarksCommandResponse
            {
                Succeeded = true,
                DeletedCount = count,
                Message = count > 0
                    ? $"{count} adet not kaydedilenlerden çıkarıldı."
                    : "Kaydedilmiş notunuz bulunmamaktadır."
            };
        }
    }
}