using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NotePool.Application.Features.Commands.Institution.AddInstitution;
using NotePool.Application.Features.Commands.Institution.RemoveInstitution;
using NotePool.Application.Features.Commands.Institution.UpdateInstitution;
using NotePool.Application.Features.Commands.Note.RemoveNote;
using NotePool.Application.Features.Queries.Institution.GetAllInstitution;
using NotePool.Application.Features.Queries.Institution.GetByIdInstitution;
using NotePool.Application.Features.Queries.Institution.SearchInstitutionsQuery;
using NotePool.Application.Features.Queries.Note.GetAllNote;
using NotePool.Application.Repositories;
using System.Net;

namespace NotePool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class InstitutionsController : ControllerBase
    {
        private readonly IInstitutionReadRepository _institutionReadRepository;
        private readonly IMediator _mediator;

        public InstitutionsController(IInstitutionReadRepository institutionReadRepository, IMediator mediator)
        {
            _institutionReadRepository = institutionReadRepository;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddInstitutionCommandRequest addInstitutionCommandRequest)
        {
            AddInstitutionCommandResponse response = await _mediator.Send(addInstitutionCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] RemoveInstitutionCommandRequest removeInstitutionCommandRequest)
        {
            RemoveInstitutionCommandResponse response = await _mediator.Send(removeInstitutionCommandRequest);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateInstitutionCommandRequest updateInstitutionCommandRequest)
        {
            UpdateInstitutionCommandResponse response = await _mediator.Send(updateInstitutionCommandRequest);
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] GetAllInstitutionQueryRequest getAllInstitutionQueryRequest)
        {
            GetAllInstitutionQueryResponse response = await _mediator.Send(getAllInstitutionQueryRequest);
            return Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetByIdInstitutionQueryRequest getByIdInstitutionQueryRequest)
        {
            GetByIdInstitutionQueryResponse response = await _mediator.Send(getByIdInstitutionQueryRequest);
            return Ok(response);
        }

        [HttpPost("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromBody] SearchInstitutionsQueryRequest request)
        {
            SearchInstitutionsQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}



//[HttpGet]
//public IActionResult GetAll()
//{
//    var institutions = _institutionReadRepository.GetAll(false)
//        .Select(i => new { i.Id, i.Name })
//        .ToList();
//    return Ok(institutions);
//}
