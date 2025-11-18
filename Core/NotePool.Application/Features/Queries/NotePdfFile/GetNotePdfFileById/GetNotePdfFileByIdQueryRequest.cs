using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.NotePdfFile.GetNotePdfFileById
{
    public class GetNotePdfFileByIdQueryRequest : IRequest<GetNotePdfFileByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
}
