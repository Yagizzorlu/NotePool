using MediatR;
using Microsoft.AspNetCore.Http;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Bookmark.GetMyBookmarkStatusForNote
{
    public class GetMyBookmarkStatusForNoteQueryHandler : IRequestHandler<GetMyBookmarkStatusForNoteQueryRequest, GetMyBookmarkStatusForNoteQueryResponse>
    {
        private readonly IBookmarkReadRepository _bookmarkReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyBookmarkStatusForNoteQueryHandler(IBookmarkReadRepository bookmarkReadRepository, IHttpContextAccessor httpContextAccessor)
        {
            _bookmarkReadRepository = bookmarkReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetMyBookmarkStatusForNoteQueryResponse> Handle(GetMyBookmarkStatusForNoteQueryRequest request, CancellationToken cancellationToken)
        {
            var loggedInUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(loggedInUserIdString))
            {
                throw new Exception("Kullanıcı Girişi Bulunamadı");
            }

            var loggedInUserId = Guid.Parse(loggedInUserIdString);
            var noteGuid = Guid.Parse(request.NoteId);

            var bookmark = await _bookmarkReadRepository.GetSingleAsync(b => b.UserId == loggedInUserId && b.NoteId == noteGuid, false);

            return new GetMyBookmarkStatusForNoteQueryResponse
            {
                IsBookmarked = bookmark != null
            };
        }
    }
}
