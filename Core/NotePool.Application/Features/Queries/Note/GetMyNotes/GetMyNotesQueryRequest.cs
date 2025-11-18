using MediatR;

namespace NotePool.Application.Features.Queries.Note.GetMyNotes
{
    public class GetMyNotesQueryRequest : IRequest<GetMyNotesQueryResponse>
    {
        public int Page { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}
