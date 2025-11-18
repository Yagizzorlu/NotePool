using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotePool.Application.Features.Commands.Course.AddCourse;
using NotePool.Application.Features.Commands.Course.RemoveCourse;
using NotePool.Application.Features.Commands.Course.UpdateCourse;
using NotePool.Application.Features.Queries.Course.GetAllCourse;
using NotePool.Application.Features.Queries.Course.GetByDepartment;
using NotePool.Application.Features.Queries.Course.GetByIdCourse;
using NotePool.Application.Features.Queries.Course.SearchCoursesByDepartment;
using NotePool.Application.Features.Queries.Department.GetByInstitutionId;
using NotePool.Application.Repositories;
using System.Net;

namespace NotePool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Admin")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseReadRepository _courseReadRepository;
        private readonly IMediator _mediator;

        public CoursesController(ICourseReadRepository courseReadRepository, IMediator mediator)
        {
            _courseReadRepository = courseReadRepository;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(AddCourseCommandRequest addCourseCommandRequest)
        {
            AddCourseCommandResponse response = await _mediator.Send(addCourseCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] RemoveCourseCommandRequest removeCourseCommandRequest)
        {
            RemoveCourseCommandResponse response = await _mediator.Send(removeCourseCommandRequest);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateCourseCommandRequest updateCourseCommandRequest)
        {
            UpdateCourseCommandResponse response = await _mediator.Send(updateCourseCommandRequest);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllCourseQueryRequest getAllCourseQueryRequest)
        {
            GetAllCourseQueryResponse response = await _mediator.Send(getAllCourseQueryRequest);
            return Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetByIdCourseQueryRequest getByIdCourseQueryRequest)
        {
            GetByIdCourseQueryResponse response = await _mediator.Send(getByIdCourseQueryRequest);
            return Ok(response);
        }

        [HttpGet("by-department/{DepartmentId}")]
        public async Task<IActionResult> Get([FromRoute] GetByDepartmentQueryRequest getByDepartmentQueryRequest)
        {
            GetByDepartmentQueryResponse response = await _mediator.Send(getByDepartmentQueryRequest);
            return Ok(response);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchCoursesByDepartmentQueryRequest request)
        {
            SearchCoursesByDepartmentQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}

