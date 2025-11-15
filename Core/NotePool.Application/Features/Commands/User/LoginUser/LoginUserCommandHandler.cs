using MediatR;
using Microsoft.AspNetCore.Identity;
using NotePool.Application.Exceptions;
using System.Threading;
using System.Threading.Tasks;
using NotePool.Application.Features.Commands.User.LoginUser;
using NotePool.Application.Abstractions.Token;
using NotePool.Application.DTOs;

namespace NotePool.Application.Features.Commands.User.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly UserManager<Domain.Entities.User> _userManager;
        readonly SignInManager<Domain.Entities.User> _signInManager;
        readonly ITokenHandler _tokenHandler;

        public LoginUserCommandHandler(UserManager<Domain.Entities.User> userManager, SignInManager<Domain.Entities.User> signInManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.User user = await _userManager.FindByNameAsync(request.UserNameOrEmail);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            }

            if (user == null)
            {
                throw new UserLoginExceptions();
            }

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                Token token = _tokenHandler.CreateAccessToken(5);
                return new LoginUserSuccessCommandResponse()
                {
                    Token = token
                };
            }

            //return new LoginUserErrorCommandResponse()
            //{
            //    Message = "Kullanıcı Adı veya Şifre Hatalı"
            //};

            throw new AuthenticationErrorException();
        }
    }
}