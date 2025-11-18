using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotePool.Application.Features.Commands.Reaction.SetReaction;
using NotePool.Application.Features.Queries.Reaction.GetMyReactionForNote;
using NotePool.Application.Features.Queries.Reaction.GetMyReactions;
using NotePool.Application.Features.Queries.Reaction.GetReactionsByNoteId;
using NotePool.Application.Features.Queries.Reaction.GetReactionsByUserId;

namespace NotePool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class ReactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> SetReaction([FromBody] SetReactionCommandRequest setReactionCommandRequest)
        {
            SetReactionCommandResponse response = await _mediator.Send(setReactionCommandRequest);
            return Ok(response);
        }

        [HttpGet("my-reaction/{NoteId}")]
        public async Task<IActionResult> GetMyReactionForNote([FromRoute] GetMyReactionForNoteQueryRequest request)
        {
            GetMyReactionForNoteQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("by-note")]
        public async Task<IActionResult> GetReactionsByNoteId([FromQuery] GetReactionsByNoteIdQueryRequest request)
        {
            GetReactionsByNoteIdQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("my-reactions")]
        public async Task<IActionResult> GetMyReactions([FromQuery] GetMyReactionsQueryRequest request)
        {
            GetMyReactionsQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("by-user /{UserId}")]
        public async Task<IActionResult> GetReactionsByUserId([FromQuery] GetReactionsByUserIdQueryRequest request)
        {
            GetReactionsByUserIdQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
