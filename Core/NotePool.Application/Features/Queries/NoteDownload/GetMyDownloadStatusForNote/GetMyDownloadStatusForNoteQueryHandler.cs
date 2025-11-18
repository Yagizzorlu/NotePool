using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Download.GetMyDownloadStatusForNote
{
    public class GetMyDownloadStatusForNoteQueryHandler : IRequestHandler<GetMyDownloadStatusForNoteQueryRequest, GetMyDownloadStatusForNoteQueryResponse>
    {
        private readonly INoteDownloadReadRepository _noteDownloadReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyDownloadStatusForNoteQueryHandler(
            INoteDownloadReadRepository noteDownloadReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _noteDownloadReadRepository = noteDownloadReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetMyDownloadStatusForNoteQueryResponse> Handle(GetMyDownloadStatusForNoteQueryRequest request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return new GetMyDownloadStatusForNoteQueryResponse { IsDownloaded = false };
            }

            var loggedInUserId = Guid.Parse(userIdString);

            var isDownloaded = await _noteDownloadReadRepository.Table
                .AnyAsync(nd =>
                    nd.UserId == loggedInUserId &&
                    nd.NoteId == request.NoteId,
                    cancellationToken);

            return new GetMyDownloadStatusForNoteQueryResponse
            {
                IsDownloaded = isDownloaded
            };
        }
    }
}
