using MediatR;
using Microsoft.AspNetCore.Identity;
using NotePool.Application.Exceptions;
using System.Threading;
using System.Threading.Tasks;
using NotePool.Application.Features.Commands.User.LoginUser;
using NotePool.Application.Abstractions.Token;
using NotePool.Application.DTOs;
using NotePool.Application.Abstractions.Services;

namespace NotePool.Application.Features.Commands.User.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly IAuthService _authService;

        public LoginUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
           var token = await _authService.LoginAsync(request.UserNameOrEmail, request.Password,1800);
           return new LoginUserSuccessCommandResponse()
            {
                Token = token
            };
        }
    }
}