using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotePool.Application.Features.Commands.Comment.CreateComment;
using NotePool.Application.Features.Commands.Comment.RemoveComment;
using NotePool.Application.Features.Commands.Comment.UpdateComment;
using NotePool.Application.Features.Queries.Comment.GetCommentsByNoteId;
using NotePool.Application.Features.Queries.Comment.GetCommentsByUserId;
using NotePool.Application.Features.Queries.Comment.GetMyComments;
using System.Threading.Tasks;

namespace NotePool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentCommandRequest request)
        {
            CreateCommentCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentCommandRequest request)
        {
            UpdateCommentCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> RemoveComment([FromRoute] RemoveCommentCommandRequest request)
        {
            RemoveCommentCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("by-note / {NoteId}")]
        public async Task<IActionResult> GetCommentsByNoteId([FromQuery] GetCommentsByNoteIdQueryRequest request)
        {
            GetCommentsByNoteIdQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("my-comments")]
        public async Task<IActionResult> GetMyComments([FromQuery] GetMyCommentsQueryRequest request)
        {
            GetMyCommentsQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("by-user/{UserId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentsByUserId([FromQuery] GetCommentsByUserIdQueryRequest getCommentsByUserIdQueryRequest)
        {
            GetCommentsByUserIdQueryResponse response = await _mediator.Send(getCommentsByUserIdQueryRequest);
            return Ok(response);
        }

    }
}
