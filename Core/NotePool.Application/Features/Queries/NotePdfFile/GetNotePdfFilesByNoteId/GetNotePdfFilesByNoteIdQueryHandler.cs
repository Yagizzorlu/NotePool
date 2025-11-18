using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.NotePdfFile.GetNotePdfFilesByNoteId
{
    public class GetNotePdfFilesByNoteIdQueryHandler : IRequestHandler<GetNotePdfFilesByNoteIdQueryRequest, GetNotePdfFilesByNoteIdQueryResponse>
    {
        readonly INotePdfFileReadRepository _notePdfFileReadRepository;

        public GetNotePdfFilesByNoteIdQueryHandler(INotePdfFileReadRepository notePdfFileReadRepository)
        {
            _notePdfFileReadRepository = notePdfFileReadRepository;
        }

        public async Task<GetNotePdfFilesByNoteIdQueryResponse> Handle(
            GetNotePdfFilesByNoteIdQueryRequest request,
            CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.NoteId, out var noteGuid))
            {
                
                return new GetNotePdfFilesByNoteIdQueryResponse { Files = new List<object>(), TotalCount = 0 };
            }

            var query = _notePdfFileReadRepository.GetAll(false)
                .Where(npf => npf.NoteId == noteGuid);

            var totalCount = await query.CountAsync(cancellationToken);

            var filesList = await query
                .OrderByDescending(npf => npf.CreatedDate) 
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .Select(pdf => new
                {
                    pdf.Id,
                    pdf.FileName,
                    pdf.Path,
                    pdf.NoteId,
                    pdf.Storage 
                })
                .ToListAsync(cancellationToken);

            return new GetNotePdfFilesByNoteIdQueryResponse
            {
                Files = filesList,
                TotalCount = totalCount
            };
        }
    }
}
