using MediatR;
using NotePool.Application.Features.Commands.Comment.CreateComment;
using System;

namespace NotePool.Application.Features.Commands.Comment.CreateComment
{
    public class CreateCommentCommandRequest : IRequest<CreateCommentCommandResponse>
    {
        public Guid NoteId { get; set; }
        public string Content { get; set; }
        public Guid? ParentId { get; set; }
    }
}
