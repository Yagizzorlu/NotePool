using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.NotePdfFile.GetNotePdfFiles
{
    public class GetNotePdfFilesQueryRequest : IRequest<List<GetNotePdfFilesQueryResponse>>
    {
        public string Id { get; set; }  
    }
}
