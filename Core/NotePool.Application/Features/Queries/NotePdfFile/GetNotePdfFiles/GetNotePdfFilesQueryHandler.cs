using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using N= NotePool.Domain.Entities;

namespace NotePool.Application.Features.Queries.NotePdfFile.GetNotePdfFiles
{
    public class GetNotePdfFilesQueryHandler : IRequestHandler<GetNotePdfFilesQueryRequest, List<GetNotePdfFilesQueryResponse>>
    {
        readonly INoteReadRepository _noteReadRepository;
        readonly IConfiguration configuration;

        public GetNotePdfFilesQueryHandler(INoteReadRepository noteReadRepository, IConfiguration configuration)
        {
            _noteReadRepository = noteReadRepository;
            this.configuration = configuration;
        }

        public async Task<List<GetNotePdfFilesQueryResponse>> Handle(
    GetNotePdfFilesQueryRequest request,
    CancellationToken cancellationToken)
        {
            // request.Id bir string; güvenli parse edelim
            if (!Guid.TryParse(request.Id, out var noteGuid))
                return new List<GetNotePdfFilesQueryResponse>();

            var note = await _noteReadRepository.Table
                .Include(n => n.NotePdfFiles)
                .FirstOrDefaultAsync(n => n.Id == noteGuid, cancellationToken);

            if (note == null || note.NotePdfFiles == null)
                return new List<GetNotePdfFilesQueryResponse>();

            return note.NotePdfFiles.Select(pdf => new GetNotePdfFilesQueryResponse
            {
                Path = pdf.Path,
                FileName = pdf.FileName,
                Id = pdf.Id,             // Guid -> Guid
                NoteId = pdf.NoteId      // Guid -> Guid
            }).ToList();
        }
    }
}
