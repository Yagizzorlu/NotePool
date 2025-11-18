using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Download.GetDownloadCountByNoteId
{
    public class GetDownloadCountByNoteIdQueryHandler : IRequestHandler<GetDownloadCountByNoteIdQueryRequest, GetDownloadCountByNoteIdQueryResponse>
    {
        private readonly INoteReadRepository _noteReadRepository;

        public GetDownloadCountByNoteIdQueryHandler(INoteReadRepository noteReadRepository)
        {
            _noteReadRepository = noteReadRepository;
        }

        public async Task<GetDownloadCountByNoteIdQueryResponse> Handle(GetDownloadCountByNoteIdQueryRequest request, CancellationToken cancellationToken)
        {
            var count = await _noteReadRepository.Table
                .Where(n => n.Id == request.NoteId)
                .Select(n => n.DownloadCount)
                .FirstOrDefaultAsync(cancellationToken);

            return new GetDownloadCountByNoteIdQueryResponse
            {
                DownloadCount = count
            };
        }
    }
}