using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NotePool.Application.Features.Queries.Bookmark.GetBookmarkCountByNoteId;

namespace NotePool.Application.Features.Queries.Bookmark.GetBookmarkCountByNoteId
{
    public class GetBookmarkCountByNoteIdQueryHandler : IRequestHandler<GetBookmarkCountByNoteIdQueryRequest, GetBookmarkCountByNoteIdQueryResponse>
    {
        private readonly IBookmarkReadRepository _bookmarkReadRepository;

        public GetBookmarkCountByNoteIdQueryHandler(IBookmarkReadRepository bookmarkReadRepository)
        {
            _bookmarkReadRepository = bookmarkReadRepository;
        }

        public async Task<GetBookmarkCountByNoteIdQueryResponse> Handle(GetBookmarkCountByNoteIdQueryRequest request, CancellationToken cancellationToken)
        {
            var noteGuid = Guid.Parse(request.NoteId);

            var count = await _bookmarkReadRepository.GetAll(false)
                .Where(b => b.NoteId == noteGuid)
                .CountAsync(cancellationToken);

            return new GetBookmarkCountByNoteIdQueryResponse
            {
                Count = count
            };
        }
    }
}
