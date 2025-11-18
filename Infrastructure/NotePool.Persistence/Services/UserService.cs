using Microsoft.AspNetCore.Identity;
using NotePool.Application.Abstractions.Services;
using NotePool.Application.DTOs.User;
using NotePool.Application.Exceptions;
using NotePool.Application.Features.Commands.User.CreateUser;
using NotePool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<Domain.Entities.User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            if (model.Password != model.PasswordConfirm)
            {
                return new CreateUserResponse
                {
                    Succeeded = false,
                    Message = "Şifreler eşleşmiyor."
                };
            }

            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid(),
                InstitutionId = model.InstitutionId,
                DepartmentId = model.DepartmentId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName,
            }, model.Password);

            CreateUserResponse response = new()
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

        public async Task UpdateRefreshToken(string refreshToken, User user, DateTime accessTokenDate, int addOnAccessTokenDate)
        {
            if(user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
                await _userManager.UpdateAsync(user);
            } 
            else 
                throw new UserLoginExceptions();
        }
    }
}
