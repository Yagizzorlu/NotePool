using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NotePool.Application.Abstractions.Services;
using NotePool.Application.Abstractions.Token;
using NotePool.Application.DTOs;
using NotePool.Application.Exceptions;
using NotePool.Application.Features.Commands.User.LoginUser;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly IConfiguration _configuration;
        readonly UserManager<Domain.Entities.User> _userManager;
        readonly ITokenHandler _tokenHandler;
        readonly IInstitutionReadRepository _institutionReadRepository;
        readonly IDepartmentReadRepository _departmentReadRepository;
        readonly SignInManager<Domain.Entities.User> _signInManager;
        readonly IUserService _userService;

        public AuthService(IConfiguration configuration, UserManager<Domain.Entities.User> userManager, ITokenHandler tokenHandler, IInstitutionReadRepository institutionReadRepository, IDepartmentReadRepository departmentReadRepository, SignInManager<Domain.Entities.User> signInManager, IUserService userService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _institutionReadRepository = institutionReadRepository;
            _departmentReadRepository = departmentReadRepository;
            _signInManager = signInManager;
            _userService = userService;
        }

        public async Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime, CancellationToken cancellationToken = default)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["ExternalLoginSettings:Google:Client_ID"] }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            var info = new UserLoginInfo("GOOGLE", payload.Subject, "GOOGLE");
            Domain.Entities.User user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    var firstInstitution = await _institutionReadRepository.GetAll(false).FirstOrDefaultAsync(cancellationToken);
                    if (firstInstitution == null)
                        throw new Exception("Sistemde kurum bulunamadı.");

                    var firstDepartment = await _departmentReadRepository.GetAll(false)
                        .Where(d => d.InstitutionId == firstInstitution.Id)
                        .FirstOrDefaultAsync(cancellationToken);
                    if (firstDepartment == null)
                        throw new Exception("Sistemde bölüm bulunamadı.");

                    string firstName = payload.Name ?? string.Empty;
                    string lastName = string.Empty;
                    if (!string.IsNullOrEmpty(payload.Name))
                    {
                        var nameParts = payload.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        if (nameParts.Length > 0)
                        {
                            firstName = nameParts[0];
                            if (nameParts.Length > 1)
                            {
                                lastName = string.Join(" ", nameParts.Skip(1));
                            }
                        }
                    }

                    user = new()
                    {
                        Id = Guid.NewGuid(),
                        Email = payload.Email,
                        UserName = payload.Email,
                        FirstName = firstName,
                        LastName = lastName,
                        InstitutionId = firstInstitution.Id,
                        DepartmentId = firstDepartment.Id
                    };

                    var identityResult = await _userManager.CreateAsync(user);
                    if (!identityResult.Succeeded)
                    {
                        throw new Exception("Kullanıcı oluşturulamadı.");
                    }
                }

                var existingLogins = await _userManager.GetLoginsAsync(user);
                if (!existingLogins.Any(l => l.LoginProvider == info.LoginProvider && l.ProviderKey == info.ProviderKey))
                {
                    var addLoginResult = await _userManager.AddLoginAsync(user, info);
                    if (!addLoginResult.Succeeded)
                    {
                        throw new Exception("Google login bilgisi eklenemedi.");
                    }
                }
            }

            Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
            await _userService.UpdateRefreshToken(token.RefreshToken,user, token.Expiration, 1800);
            return token;
        }

        public async Task<Token> LoginAsync(string userNameOrEmail, string password, int accessTokenLifeTime)
        {
            Domain.Entities.User user = await _userManager.FindByNameAsync(userNameOrEmail);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(userNameOrEmail);
            }

            if (user == null)
            {
                throw new UserLoginExceptions();
            }

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded)
            {
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 1800);
                return token;
            }

            //return new LoginUserErrorCommandResponse()
            //{
            //    Message = "Kullanıcı Adı veya Şifre Hatalı"
            //};

            throw new AuthenticationErrorException();
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
            User? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                Token token = _tokenHandler.CreateAccessToken(15, user);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 1800);
                return token;
            }
            else
                throw new UserLoginExceptions();
        }
    }
}
