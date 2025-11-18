using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotePool.Application.Features.Commands.NotePdfFile.RemoveNotePdfFile;
using NotePool.Application.Features.Commands.NotePdfFile.UploadNotePdfFile;
using NotePool.Application.Features.Commands.NotePdfFile.UploadNotePdfFile.NotePool.Application.Features.Commands.NotePdfFile.UploadNotePdfFile;
using NotePool.Application.Features.Queries.NotePdfFile.GetAllNotePdfFiles;
using NotePool.Application.Features.Queries.NotePdfFile.GetNotePdfFileById;
using NotePool.Application.Features.Queries.NotePdfFile.GetNotePdfFilesByNoteId;

namespace NotePool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class NotePdfFilesController : ControllerBase
    {
        readonly IMediator _mediator;
        public NotePdfFilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete("{NoteId}/file/{FileId}")]
        public async Task<IActionResult> RemoveNoteFile([FromRoute] Guid noteId , [FromRoute] Guid fileId)
        {
            var request = new RemoveNotePdfFileCommandRequest
            {
                Id = noteId.ToString(),
                FileId = fileId.ToString()
            };

            RemoveNotePdfFileCommandResponse response = await _mediator.Send(request);
            return Ok(response); 
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadNotePdf([FromForm] UploadNotePdfFileCommandRequest request)
        {
            UploadNotePdfFileCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("list/{NoteId}")] 
        public async Task<IActionResult> GetNotePdfFilesByNoteId([FromRoute] GetNotePdfFilesByNoteIdQueryRequest request)
        {
            GetNotePdfFilesByNoteIdQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetNotePdfFileById([FromRoute] Guid Id)
        {
            var request = new GetNotePdfFileByIdQueryRequest { Id = Id };
            GetNotePdfFileByIdQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotePdfFiles([FromQuery] GetAllNotePdfFilesQueryRequest request)
        {
            GetAllNotePdfFilesQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

    }
}
