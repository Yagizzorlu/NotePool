using MediatR;
using NotePool.Application.Features.Commands.Comment.RemoveComment;
using System;

namespace NotePool.Application.Features.Commands.Comment.RemoveComment
{
    public class RemoveCommentCommandRequest : IRequest<RemoveCommentCommandResponse>
    {
        public string Id { get; set; }
    }
}
