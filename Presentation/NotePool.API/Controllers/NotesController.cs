using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Abstractions.Storage;
using NotePool.Application.Features.Commands.Note.CreateNote;
using NotePool.Application.Features.Commands.Note.RemoveNote;
using NotePool.Application.Features.Commands.Note.UpdateNote;
using NotePool.Application.Features.Commands.NotePdfFile.RemoveNotePdfFile;
using NotePool.Application.Features.Commands.NotePdfFile.UploadNotePdfFile;
using NotePool.Application.Features.Queries.Note.GetAllNote;
using NotePool.Application.Features.Queries.Note.GetByCourseId;
using NotePool.Application.Features.Queries.Note.GetByDepartmentId;
using NotePool.Application.Features.Queries.Note.GetByIdNote;
using NotePool.Application.Features.Queries.Note.GetByInstitutionId;
using NotePool.Application.Features.Queries.Note.GetByUserId;
using NotePool.Application.Features.Queries.Note.GetMyNotes;
using NotePool.Application.Features.Queries.Note.GetNotesByUserId;
using NotePool.Application.Features.Queries.Note.SearchNotes;
using NotePool.Application.Repositories;
using NotePool.Application.RequestParameters;
using NotePool.Application.ViewModels.Notes;
using NotePool.Domain.Entities;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace NotePool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class NotesController : ControllerBase
    {

        readonly IMediator _mediator;

        public NotesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]GetAllNoteQueryRequest getAllNoteQueryRequest)
        {
            GetAllNoteQueryResponse response = await _mediator.Send(getAllNoteQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateNoteCommandRequest createNoteCommandRequest)
        {
            CreateNoteCommandResponse response = await _mediator.Send(createNoteCommandRequest);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateNoteCommandRequest updateNoteCommandRequest)
        {
            UpdateNoteCommandResponse response = await _mediator.Send(updateNoteCommandRequest);
            return Ok(response);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] RemoveNoteCommandRequest removeNoteCommandRequest)
        {
            RemoveNoteCommandResponse response = await _mediator.Send(removeNoteCommandRequest);
            return Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetByIdNoteQueryRequest getByIdNoteQueryRequest)
        {
            GetByIdNoteQueryResponse response = await _mediator.Send(getByIdNoteQueryRequest);
            return Ok(response);
        }

        [HttpPost("create-with-pdf")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateWithPdf([FromForm] CreateNoteCommandRequest createNoteCommandRequest)
        {
            var response = await _mediator.Send(createNoteCommandRequest);
            return Ok(response);
        }

        [HttpGet("by-course/{CourseId}")]
        public async Task<IActionResult> Get([FromRoute] GetByCourseIdQueryRequest getByCourseIdQueryRequest)
        {
            GetByCourseIdQueryResponse response = await _mediator.Send(getByCourseIdQueryRequest);
            return Ok(response);
        }

        [HttpGet("by-department/{DepartmentId}")]
        public async Task<IActionResult> Get([FromRoute] GetByDepartmentIdQueryRequest getByDepartmentIdQueryRequest)
        {
            GetByDepartmentIdQueryResponse response = await _mediator.Send(getByDepartmentIdQueryRequest);
            return Ok(response);
        }

        [HttpGet("by-institution/{InstitutionId}")]
        public async Task<IActionResult> Get([FromRoute] GetByInstitutionIdQueryRequest getByInstitutionIdQueryRequest)
        {
            GetByInstitutionIdQueryResponse response = await _mediator.Send(getByInstitutionIdQueryRequest);
            return Ok(response);
        }

        [HttpGet("by-user/{UserId}")]
        public async Task<IActionResult> Get([FromRoute] GetByUserIdQueryRequest getByUserIdQueryRequest)
        {
            GetByUserIdQueryResponse response = await _mediator.Send(getByUserIdQueryRequest);
            return Ok(response);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchNotesQueryRequest request)
        {
            SearchNotesQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("my-notes")]
        public async Task<IActionResult> GetMyNotes([FromQuery] GetMyNotesQueryRequest request)
        {
            GetMyNotesQueryResponse response = await _mediator.Send(request);

            return Ok(response);
        }
    }
}

    //if (noteId == Guid.Empty)
    //    return BadRequest("noteId is required.");

    //if (files == null || files.Count == 0)
    //    return BadRequest("Dosya yüklenmedi.");


    //var fileCollection = new FormFileCollection();
    //foreach (var f in files) fileCollection.Add(f);

    //var datas = await _storageService.UploadAsync("note-files", fileCollection);
    //if (datas == null || datas.Count == 0)
    //    return BadRequest("Yükleme başarısız.");

    //var pdfs = datas.Select(d => new NotePdfFile
    //{
    //    NoteId = noteId,
    //    FileName = d.fileName,
    //    Path = d.pathOrContainerName,
    //    Storage = _storageService.StorageName
    //}).ToList();

    //await _notePdfFileWriteRepository.AddRangeAsync(pdfs);
    //return Ok(new { message = "PDF yüklendi", count = pdfs.Count });
