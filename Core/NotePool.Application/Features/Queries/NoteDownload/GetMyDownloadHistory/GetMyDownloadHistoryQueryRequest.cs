using MediatR;

namespace NotePool.Application.Features.Queries.Download.GetMyDownloadHistory
{
    public class GetMyDownloadHistoryQueryRequest : IRequest<GetMyDownloadHistoryQueryResponse>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}
