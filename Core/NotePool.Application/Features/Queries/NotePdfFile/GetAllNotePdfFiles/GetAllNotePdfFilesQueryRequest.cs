using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.NotePdfFile.GetAllNotePdfFiles
{
    public class GetAllNotePdfFilesQueryRequest : IRequest<GetAllNotePdfFilesQueryResponse>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}
