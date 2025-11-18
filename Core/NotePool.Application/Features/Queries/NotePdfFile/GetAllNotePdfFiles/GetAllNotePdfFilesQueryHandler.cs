using MediatR;
using NotePool.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.NotePdfFile.GetAllNotePdfFiles
{
    public class GetAllNotePdfFilesQueryHandler : IRequestHandler<GetAllNotePdfFilesQueryRequest, GetAllNotePdfFilesQueryResponse>
    {
        private readonly INotePdfFileReadRepository _notePdfFileReadRepository;

        public GetAllNotePdfFilesQueryHandler(INotePdfFileReadRepository notePdfFileReadRepository)
        {
            _notePdfFileReadRepository = notePdfFileReadRepository;
        }

        public async Task<GetAllNotePdfFilesQueryResponse> Handle(GetAllNotePdfFilesQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _notePdfFileReadRepository.GetAll(false);

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
                    pdf.Storage,
                    pdf.NoteId,
                    pdf.CreatedDate
                })
                .ToListAsync(cancellationToken);

            return new GetAllNotePdfFilesQueryResponse
            {
                Files = filesList,
                TotalCount = totalCount
            };
        }
    }
}
