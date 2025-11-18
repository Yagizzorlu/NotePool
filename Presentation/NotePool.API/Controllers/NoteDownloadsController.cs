using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotePool.Application.Features.Commands.Download.RemoveNoteDownload;
using NotePool.Application.Features.Commands.NoteDownload.CreateNoteDownload;
using NotePool.Application.Features.Queries.Download.GetDownloadCountByNoteId;
using NotePool.Application.Features.Queries.Download.GetMyDownloadHistory;
using NotePool.Application.Features.Queries.Download.GetMyDownloadStatusForNote;
using NotePool.Application.Features.Queries.Download.GetNoteDownloadsByNoteId;
using NotePool.Application.Features.Queries.Download.GetNoteDownloadsByUserId;

namespace NotePool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class NoteDownloadsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NoteDownloadsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("download/{NoteId}")]
        public async Task<IActionResult> CreateNoteDownload([FromRoute] Guid noteId)
        {
            var request = new CreateNoteDownloadCommandRequest { NoteId = noteId };

            var response = await _mediator.Send(request);

            if (!response.IsSuccess || response.FileContents == null)
            {
                return BadRequest("İndirme işlemi sırasında bir hata oluştu veya dosya içeriği boş.");
            }

            return File(response.FileContents, "application/pdf", response.FileName);
        }

        [HttpDelete("download/{Id}")]
        public async Task<IActionResult> RemoveNoteDownload([FromRoute] Guid id)
        {
            var request = new RemoveNoteDownloadCommandRequest { Id = id };

            RemoveNoteDownloadCommandResponse response = await _mediator.Send(request);

            if (!response.IsSuccess)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("{NoteId}/download/count")]
        public async Task<IActionResult> GetDownloadCount([FromRoute] Guid noteId)
        {
            var request = new GetDownloadCountByNoteIdQueryRequest { NoteId = noteId };

            GetDownloadCountByNoteIdQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("my-downloads")]
        public async Task<IActionResult> GetMyDownloadHistory([FromQuery] GetMyDownloadHistoryQueryRequest request)
        {
            GetMyDownloadHistoryQueryResponse response = await _mediator.Send(request);

            return Ok(response);
        }

        [HttpGet("{NoteId}/download/status")]
        public async Task<IActionResult> GetMyDownloadStatus([FromRoute] Guid noteId)
        {
            var request = new GetMyDownloadStatusForNoteQueryRequest { NoteId = noteId };

            GetMyDownloadStatusForNoteQueryResponse response = await _mediator.Send(request);

            return Ok(response);
        }


        [HttpGet("notes/ {NoteId}")]
        public async Task<IActionResult> GetNoteDownloadsByNoteId(
            [FromRoute] GetNoteDownloadsByNoteIdQueryRequest request)
        {
            GetNoteDownloadsByNoteIdQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("users/{UserId}")]
        public async Task<IActionResult> GetNoteDownloadsByUserId(
            [FromRoute] GetNoteDownloadsByUserIdQueryRequest request) 
        {
            GetNoteDownloadsByUserIdQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

    }
}
