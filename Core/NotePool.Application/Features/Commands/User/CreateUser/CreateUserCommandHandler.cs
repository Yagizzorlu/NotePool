using MediatR;
using Microsoft.AspNetCore.Identity;
using NotePool.Application.Features.Commands.User.CreateUser;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.User.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly UserManager<Domain.Entities.User> _userManager;

        public CreateUserCommandHandler(UserManager<Domain.Entities.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            if (request.Password != request.PasswordConfirm)
            {
                return new CreateUserCommandResponse
                {
                    Succeeded = false,
                    Message = "Şifreler eşleşmiyor."
                };
            }

            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid(),
                InstitutionId = request.InstitutionId,
                DepartmentId = request.DepartmentId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
            }, request.Password);

            CreateUserCommandResponse response = new() 
            { 
                Succeeded = result.Succeeded,
                Message = string.Empty
            };

            if (result.Succeeded)
            {
                response.Message = "Kullanıcı Başarıyla Oluşturuldu.";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    response.Message += $"{error.Code} - {error.Description}\n";
                }
            }

            return response;
        }
    }
}
//result.Errors.First()
//throw new UserCreateExceptions();

