using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotePool.Application.Features.Commands.User.CreateUser;
using NotePool.Application.Features.Commands.User.GoogleLogin;
using NotePool.Application.Features.Commands.User.LoginUser;
using NotePool.Application.Features.Commands.User.RemoveUser;
using NotePool.Application.Features.Commands.User.UpdateMyProfile;
using NotePool.Application.Features.Commands.User.UpdateUser;
using NotePool.Application.Features.Commands.User.UploadProfileImage;
using NotePool.Application.Features.Queries.User.GetAllUsers;
using NotePool.Application.Features.Queries.User.GetMyProfile;
using NotePool.Application.Features.Queries.User.GetPublicUserProfile;
using NotePool.Application.Features.Queries.User.GetUserByDepartmentId;
using NotePool.Application.Features.Queries.User.GetUserById;
using NotePool.Application.Features.Queries.User.GetUserByInstitutionId;

namespace NotePool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse response = await _mediator.Send(createUserCommandRequest);

            if (response.Succeeded)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpGet("my-profile")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> GetMyProfile(GetMyProfileQueryRequest getMyProfileQueryRequest)
        {
            GetMyProfileQueryResponse response = await _mediator.Send(new GetMyProfileQueryRequest());
            return Ok(response);
        }

        [HttpPut("my-profile")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateMyProfileCommandRequest request)
        {
            UpdateMyProfileCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("public-profile / {Id}")]
        public async Task<IActionResult> GetUserProfile([FromRoute]GetPublicUserProfileQueryRequest getPublicUserProfileQueryRequest)
        {
            GetPublicUserProfileQueryResponse response = await _mediator.Send(getPublicUserProfileQueryRequest);
            return Ok(response);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Get([FromQuery] GetAllUsersQueryRequest getAllUsersQueryRequest)
        {
            GetAllUsersQueryResponse response = await _mediator.Send(getAllUsersQueryRequest);
            return Ok(response);
        }

        [HttpGet("{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Get([FromRoute] GetUserByIdQueryRequest getUserByIdQueryRequest)
        {
            GetUserByIdQueryResponse response = await _mediator.Send(getUserByIdQueryRequest);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateUserCommandRequest updateUserCommandRequest)
        {
            UpdateUserCommandResponse response = await _mediator.Send(updateUserCommandRequest);
            return Ok(response);
        }

        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] RemoveUserCommandRequest removeUserCommandRequest)
        {
            RemoveUserCommandResponse response = await _mediator.Send(removeUserCommandRequest);
            return Ok(response);
        }

        [HttpGet("by-institution/{InstitutionId}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Get([FromRoute] GetUserByInstitutionIdQueryRequest getUserByInstitutionIdQueryRequest)
        {
            GetUserByInstitutionIdQueryResponse response = await _mediator.Send(getUserByInstitutionIdQueryRequest);
            return Ok(response);
        }

        [HttpGet("by-department/{DepartmentId}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Get([FromRoute] GetUserByDepartmentIdQueryRequest getUserByDepartmentIdQueryRequest)
        {
            GetUserByDepartmentIdQueryResponse response = await _mediator.Send(getUserByDepartmentIdQueryRequest);
            return Ok(response);
        }

        [HttpPost("my-profile/upload-image")]
        [Authorize(AuthenticationSchemes = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadProfileImage([FromForm] UploadProfileImageCommandRequest request)
        {
            UploadProfileImageCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
