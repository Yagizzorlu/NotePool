using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotePool.Application.Features.Commands.Bookmark.BookmarkNote;
using NotePool.Application.Features.Commands.Bookmark.DeleteAllMyBookmarks;
using NotePool.Application.Features.Queries.Bookmark.GetBookmarkCountByNoteId;
using NotePool.Application.Features.Queries.Bookmark.GetMyBookmarks;
using NotePool.Application.Features.Queries.Bookmark.GetMyBookmarkStatusForNote;

namespace NotePool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class BookmarksController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BookmarksController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> BookmarkNote([FromBody] BookmarkNoteCommandRequest request)
        {
            BookmarkNoteCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("status/{NoteId}")]
        public async Task<IActionResult> GetMyBookmarkStatus([FromRoute] GetMyBookmarkStatusForNoteQueryRequest request)
        {
            GetMyBookmarkStatusForNoteQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("my-bookmarks")]
        public async Task<IActionResult> GetMyBookmarks([FromQuery] GetMyBookmarksQueryRequest request)
        {
            GetMyBookmarksQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("count/{NoteId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBookmarkCount([FromRoute] GetBookmarkCountByNoteIdQueryRequest request)
        {
            GetBookmarkCountByNoteIdQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("my-bookmarks")]
        public async Task<IActionResult> DeleteAllMyBookmarks(DeleteAllMyBookmarksCommandRequest request)
        {
            DeleteAllMyBookmarksCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
