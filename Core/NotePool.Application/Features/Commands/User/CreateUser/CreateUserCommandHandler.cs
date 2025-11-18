using MediatR;
using Microsoft.AspNetCore.Identity;
using NotePool.Application.Abstractions.Services;
using NotePool.Application.DTOs.User;
using NotePool.Application.Features.Commands.User.CreateUser;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.User.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {

            CreateUserResponse response = await _userService.CreateAsync(new()
            {
                InstitutionId = request.InstitutionId,
                DepartmentId = request.DepartmentId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                Password = request.Password,
                PasswordConfirm = request.PasswordConfirm
            });

            return new()
            {
                Message = response.Message,
                Succeeded = response.Succeeded
            };
        }
    }
}
//result.Errors.First()
//throw new UserCreateExceptions();

