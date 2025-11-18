using MediatR;
using NotePool.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.NotePdfFile.GetNotePdfFileById
{
    public class GetNotePdfFileByIdQueryHandler : IRequestHandler<GetNotePdfFileByIdQueryRequest, GetNotePdfFileByIdQueryResponse>
    {
        private readonly INotePdfFileReadRepository _notePdfFileReadRepository;

        public GetNotePdfFileByIdQueryHandler(INotePdfFileReadRepository notePdfFileReadRepository)
        {
            _notePdfFileReadRepository = notePdfFileReadRepository;
        }

        public async Task<GetNotePdfFileByIdQueryResponse> Handle(GetNotePdfFileByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var file = await _notePdfFileReadRepository.Table
                .Where(npf => npf.Id == request.Id)
                .Select(pdf => new GetNotePdfFileByIdQueryResponse
                {
                    Id = pdf.Id,
                    FileName = pdf.FileName,
                    Path = pdf.Path,
                    Storage = pdf.Storage,
                    NoteId = pdf.NoteId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (file == null)
            {
                throw new KeyNotFoundException($"Dosya kaydı ({request.Id}) bulunamadı.");
            }

            return file;
        }
    }
}
