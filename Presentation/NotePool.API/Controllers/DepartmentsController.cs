using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotePool.Application.Features.Commands.Department.AddDepartment;
using NotePool.Application.Features.Commands.Department.RemoveDepartment;
using NotePool.Application.Features.Commands.Department.UpdateDepartment;
using NotePool.Application.Features.Queries.Department.GetAllDepartment;
using NotePool.Application.Features.Queries.Department.GetByIdDepartment;
using NotePool.Application.Features.Queries.Department.GetByInstitutionId;
using NotePool.Application.Features.Queries.Department.SearchDepartmentsByInstitution;
using NotePool.Application.Repositories;
using System.Net;

namespace NotePool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentReadRepository _departmentReadRepository;
        private readonly IMediator _mediator;

        public DepartmentsController(IDepartmentReadRepository departmentReadRepository, IMediator mediator)
        {
            _departmentReadRepository = departmentReadRepository;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddDepartmentCommandRequest addDepartmentCommandRequest)
        {
            AddDepartmentCommandResponse response = await _mediator.Send(addDepartmentCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] RemoveDepartmentCommandRequest removeDepartmentCommandRequest)
        {
            RemoveDepartmentCommandResponse response = await _mediator.Send(removeDepartmentCommandRequest);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateDepartmentCommandRequest updateDepartmentCommandRequest)
        {
            UpdateDepartmentCommandResponse response = await _mediator.Send(updateDepartmentCommandRequest);
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromQuery] GetAllDepartmentQueryRequest getAllDepartmentQueryRequest)
        {
            GetAllDepartmentQueryResponse response = await _mediator.Send(getAllDepartmentQueryRequest);
            return Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetByIdDepartmentQueryRequest getByIdDepartmentQueryRequest)
        {
            GetByIdDepartmentQueryResponse response = await _mediator.Send(getByIdDepartmentQueryRequest);
            return Ok(response);
        }

        [HttpGet("by-institution/{InstitutionId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromRoute] GetByInstitutionIdQueryRequest getByInstitutionIdQueryRequest)
        {
            GetByInstitutionIdQueryResponse response = await _mediator.Send(getByInstitutionIdQueryRequest);
            return Ok(response);
        }

        [HttpPost("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromBody] SearchDepartmentsByInstitutionQueryRequest request)
        {
            SearchDepartmentsByInstitutionQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
