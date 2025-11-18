using MediatR;
using NotePool.Application.Features.Commands.Comment.UpdateComment;
using System;

namespace NotePool.Application.Features.Commands.Comment.UpdateComment
{
    public class UpdateCommentCommandRequest : IRequest<UpdateCommentCommandResponse>
    {
        public string Id { get; set; }
        public string Content { get; set; }
    }
}