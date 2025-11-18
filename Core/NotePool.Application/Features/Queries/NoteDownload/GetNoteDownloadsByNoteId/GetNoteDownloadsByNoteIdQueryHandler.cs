using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Download.GetNoteDownloadsByNoteId
{
    public class GetNoteDownloadsByNoteIdQueryHandler : IRequestHandler<GetNoteDownloadsByNoteIdQueryRequest, GetNoteDownloadsByNoteIdQueryResponse>
    {
        private readonly INoteDownloadReadRepository _noteDownloadReadRepository;

        public GetNoteDownloadsByNoteIdQueryHandler(INoteDownloadReadRepository noteDownloadReadRepository)
        {
            _noteDownloadReadRepository = noteDownloadReadRepository;
        }

        public async Task<GetNoteDownloadsByNoteIdQueryResponse> Handle(GetNoteDownloadsByNoteIdQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _noteDownloadReadRepository.GetAll(false)
                .Where(nd => nd.NoteId == request.NoteId);

            var totalCount = await query.CountAsync(cancellationToken);

            var downloadsList = await query
                .Include(nd => nd.User)
                .OrderByDescending(nd => nd.CreatedDate)
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .Select(nd => new
                {
                    DownloadId = nd.Id,
                    DownloadDate = nd.CreatedDate,
                    UserId = nd.UserId,
                    UserName = nd.User != null ? nd.User.UserName : "Bilinmeyen Kullanıcı",
                    UserEmail = nd.User != null ? nd.User.Email : null
                })
                .ToListAsync(cancellationToken);

            return new GetNoteDownloadsByNoteIdQueryResponse
            {
                Downloads = downloadsList,
                TotalCount = totalCount
            };
        }
    }
}
