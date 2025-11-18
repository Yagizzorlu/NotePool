using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NotePool.Application.Abstractions.Storage;
using NotePool.Domain.Entities;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using NotePool.Application.Features.Commands.User.UploadProfileImage;

namespace NotePool.Application.Features.Commands.User.UploadProfileImage
{
    public class UploadProfileImageCommandHandler : IRequestHandler<UploadProfileImageCommandRequest, UploadProfileImageCommandResponse>
    {
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStorageService _storageService;

        public UploadProfileImageCommandHandler(
            UserManager<Domain.Entities.User> userManager,
            IHttpContextAccessor httpContextAccessor,
            IStorageService storageService)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _storageService = storageService;
        }

        public async Task<UploadProfileImageCommandResponse> Handle(UploadProfileImageCommandRequest request, CancellationToken cancellationToken)
        {
            var loggedInUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdString))
                throw new Exception("Kullanıcı girişi bulunamadı.");

            var user = await _userManager.FindByIdAsync(loggedInUserIdString);
            if (user == null)
                throw new Exception("Kullanıcı profili bulunamadı.");

            var uploadedList = await _storageService.UploadAsync("profile-pictures", new FormFileCollection { request.File });
            var (fileName, path) = uploadedList.FirstOrDefault();

            if (string.IsNullOrEmpty(path))
                throw new Exception("Dosya yüklenemedi.");

            user.ProfileImage = path;

            IdentityResult result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception("Profil resmi veritabanına kaydedilemedi.");

            return new UploadProfileImageCommandResponse
            {
                Succeeded = true,
                NewProfileImageUrl = path
            };
        }
    }
}
