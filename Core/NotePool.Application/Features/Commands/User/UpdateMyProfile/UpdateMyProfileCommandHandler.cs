using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.User.UpdateMyProfile
{
    public class UpdateMyProfileCommandHandler : IRequestHandler<UpdateMyProfileCommandRequest, UpdateMyProfileCommandResponse>
    {
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateMyProfileCommandHandler(
            UserManager<Domain.Entities.User> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UpdateMyProfileCommandResponse> Handle(UpdateMyProfileCommandRequest request, CancellationToken cancellationToken)
        {
            var loggedInUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdString))
            {
                throw new Exception("Kullanıcı girişi bulunamadı (Token [Jeton] yok veya geçersiz).");
            }

            var user = await _userManager.FindByIdAsync(loggedInUserIdString);

            if (user == null)
            {
                throw new Exception("Kullanıcı profili veritabanında bulunamadı.");
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.UserName = request.UserName;
            user.ProfileImage = request.ProfileImage;

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join("\n", result.Errors.Select(e => e.Description));
                throw new Exception($"Profil güncellenemedi: {errors}");
            }

            return new UpdateMyProfileCommandResponse
            {
                Success = true,
                Message = "Profiliniz başarıyla güncellendi."
            };
        }
    }
}
